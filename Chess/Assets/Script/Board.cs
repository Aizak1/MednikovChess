using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
public enum GameState
{
    NotStarted,
    Continues,
    Finished
}
public enum TurnState
{
    Obvious,
    Check,
    CheckAndMate,
    Pat
}
public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private GameObject moveHighlighter;
    [SerializeField] private ModelMathcer modelMatcher;
    [SerializeField] private SFX sfx;
    [SerializeField] private GameObject vfx;
    private Vector3 boardInitialCoordinates = new Vector3(0.05999994f, 0, 7.008001f);
    private BoardState initialState;
    private Figure selectedFigure;
    private List<Move> currentTurnMoves;
    public Vector2Int PreviousMoveFinalPosition { get; private set; }
    public bool IsWhiteTurn { get; private set; }
    public List<Figure> FiguresOnBoard { get; private set; }
    public bool OnPause { get; private set; }
    public  TurnState CurrentTurnState { get; private set; }
    public static GameState CurrentGameState { get; set; }
    private void Start()
    {
        transform.position = boardInitialCoordinates;
        FiguresOnBoard = new List<Figure>();
        if (CurrentGameState == GameState.NotStarted)
            initialState = saveLoader.LoadState("Initial.json");
        else if (CurrentGameState == GameState.Continues)
            initialState = saveLoader.LoadState("Save.json");
        for (int i = 0; i < initialState.figuresData.Length; i++)
            GenerateFigure(modelMatcher.KindModelPairs[Tuple.Create(initialState.figuresData[i].kind, initialState.figuresData[i].isWhite)], initialState.figuresData[i]);
        IsWhiteTurn = initialState.isWhiteTurn;
        CurrentTurnState = initialState.currentTurnState;
        PreviousMoveFinalPosition = initialState.previousMoveFinalPosition;
    }
    private void Update()
    {
        if (CurrentGameState == GameState.Finished || OnPause)
            return;
        Vector2Int mouseDownPosition = Vector2Int.zero - Vector2Int.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.565f, 0.47f);
            mouseDownPosition = new Vector2Int((int)(hit.point.x + cellOffset.x), (int)(hit.point.z + cellOffset.y));
            tileHighlighter.SetActive(true);
            Vector3 tileHighlighterOffset = new Vector3(0.04f, 0.126f, -0.03f);
            tileHighlighter.transform.position = new Vector3(mouseDownPosition.x + tileHighlighterOffset.x, tileHighlighterOffset.y, mouseDownPosition.y + tileHighlighterOffset.z);
            if (Input.GetMouseButtonDown(0))
            {
                currentTurnMoves = GetAllCurrentTurnMoves(FiguresOnBoard, PreviousMoveFinalPosition, CurrentTurnState);
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                bool figureIsAbleToMove = currentTurnMoves.Select(move => move.CurrentFigure).ToList().Contains(figure);
                if (figure != null && figure.Data.isWhite == IsWhiteTurn && figureIsAbleToMove)
                {
                    selectedFigure = figure;
                    var currentFigureMoves = currentTurnMoves.Where(move => move.CurrentFigure == selectedFigure).ToList();
                    foreach (var move in currentFigureMoves)
                    {
                        Vector3 moveHighlighterPosition = new Vector3(move.FinalPosition.x + tileHighlighterOffset.x, tileHighlighterOffset.y, move.FinalPosition.y + tileHighlighterOffset.z);
                        Instantiate(moveHighlighter,moveHighlighterPosition, Quaternion.identity);
                    }
                    sfx.PlayPicKSound();
                }
            }
            Vector3 optimalHightForSelectedFigure = 2.3f * Vector3.up;
            if (selectedFigure != null)
                selectedFigure.transform.position = hit.point + optimalHightForSelectedFigure;
            
                
        }
        if (Input.GetMouseButtonUp(0))
            if (selectedFigure != null)
                TryMakeTurn(new Move(selectedFigure, mouseDownPosition));
    }
    private List<Move> GetAllCurrentTurnMoves(List<Figure> figuresOnBoard,Vector2Int previousMoveFinalPosition,TurnState currentTurnState)
    {
        List<Move> possibleMoves = new List<Move>();
        var myFigures = figuresOnBoard.Where(figure => figure.Data.isWhite == IsWhiteTurn).ToArray();
        foreach (var figure in myFigures)
        {
            for (int z = 0; z < 8; z++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector2Int finalPosition = new Vector2Int(x, z);
                    if (IsAbleToMove(figure,figuresOnBoard, previousMoveFinalPosition, finalPosition,currentTurnState))
                    {
                        var figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == finalPosition);
                        List<Figure> boardCopy = new List<Figure>(figuresOnBoard);
                        if (figureToCapture != null)
                        {
                            boardCopy.Remove(figureToCapture);
                        }
                        int indexOfFigure = boardCopy.IndexOf(figure);
                        FigureData figureDataBackUp = boardCopy[indexOfFigure].Data;
                        boardCopy[indexOfFigure].Data.position = finalPosition;
                        if (!IsCheck(boardCopy,previousMoveFinalPosition,currentTurnState))
                            possibleMoves.Add(new Move(figure, finalPosition));
                        boardCopy[indexOfFigure].Data = figureDataBackUp;
                    }
                }
            }
        }
        return possibleMoves;
    }
    private bool IsCheck(List<Figure> boardCopy, Vector2Int previousMoveFinalPosition, TurnState currentTurnState)
    {
        var opponentTurnFigures = boardCopy.Where(figure => figure.Data.isWhite != IsWhiteTurn).ToArray();
        var currentKing = boardCopy.FirstOrDefault(figure => figure.Data.kind == Kind.King && figure.Data.isWhite == IsWhiteTurn);
        foreach (var opponentFigure in opponentTurnFigures)
        {
            if (IsAbleToMove(opponentFigure,boardCopy, previousMoveFinalPosition, currentKing.Data.position,currentTurnState))
                return true;
        }
        return false;
    }
    private void TryMakeTurn(Move move)
    {
        var moveTiles = GameObject.FindGameObjectsWithTag("MoveTile");
        foreach (var moveTile in moveTiles)
        {
            Destroy(moveTile.gameObject);
        }
        if (IsCheck(FiguresOnBoard, PreviousMoveFinalPosition, CurrentTurnState))
            CurrentTurnState = TurnState.Check;
        else
            CurrentTurnState = TurnState.Obvious;
        Vector2Int initialPosition = move.CurrentFigure.Data.position;
        var possibleMove = currentTurnMoves.FirstOrDefault(validMove => validMove.CurrentFigure == move.CurrentFigure && validMove.FinalPosition == move.FinalPosition);
        if (possibleMove == null)
        {
            move.CurrentFigure.transform.position = new Vector3(initialPosition.x, 0, initialPosition.y);
            selectedFigure = null;
            sfx.PlayDropSound();
            return;
        }
        if (Mathf.Abs(move.CurrentFigure.Data.position.x - move.FinalPosition.x) == 2 && move.CurrentFigure.Data.kind == Kind.King)
            MakeCastling(move);
        var figureToCapture = FiguresOnBoard.FirstOrDefault(figure => figure.Data.position == move.FinalPosition);
        #region Проверка на взятие на проходе
        if (figureToCapture == null && move.CurrentFigure.Data.kind == Kind.Pawn)
        {
            if (Mathf.Abs(move.CurrentFigure.Data.position.y - move.FinalPosition.y) == 1 && Mathf.Abs(move.CurrentFigure.Data.position.x - move.FinalPosition.x) == 1)
            {
                int pawnPassageYLocation = move.FinalPosition.y == 5 ? pawnPassageYLocation = 4 : pawnPassageYLocation = 3;
                figureToCapture = FiguresOnBoard.FirstOrDefault(figure => figure.Data.position == new Vector2Int(move.FinalPosition.x, pawnPassageYLocation)
                && figure.Data.isWhite != IsWhiteTurn && figure.Data.kind == Kind.Pawn);
            }
        }
        #endregion
        if (figureToCapture != null)
        {
            FiguresOnBoard.Remove(figureToCapture);
            Instantiate(vfx, figureToCapture.transform.position, Quaternion.identity);
            Destroy(figureToCapture.gameObject);
            sfx.PlayKillSound();
        }
        else
        {
            sfx.PlayDropSound();
        }
        move.CurrentFigure.Data.position = move.FinalPosition;
        move.CurrentFigure.transform.position = new Vector3(move.FinalPosition.x, 0, move.FinalPosition.y);
        move.CurrentFigure.Data.turnCount++;
        selectedFigure = null;
        IsWhiteTurn = !IsWhiteTurn;
        if (move.CurrentFigure.Data.kind == Kind.Pawn && (move.FinalPosition.y == 7 || move.FinalPosition.y == 0))
            OnPause = true;
        PreviousMoveFinalPosition = move.FinalPosition;
        if (IsCheck(FiguresOnBoard, PreviousMoveFinalPosition, CurrentTurnState))
            CurrentTurnState = TurnState.Check;
        else
            CurrentTurnState = TurnState.Obvious;
        if (FiguresOnBoard.Count == 2 || (FiguresOnBoard.Count == 3 && FiguresOnBoard.FirstOrDefault(figure => figure.Data.kind == Kind.Queen || figure.Data.kind == Kind.Rook)==null))
        {
            CurrentTurnState = TurnState.Pat;
            CurrentGameState = GameState.Finished;
        }
        if (GetAllCurrentTurnMoves(FiguresOnBoard, PreviousMoveFinalPosition, CurrentTurnState).Count == 0)
        {
            if (CurrentTurnState == TurnState.Check)
                CurrentTurnState = TurnState.CheckAndMate;
            else
                CurrentTurnState = TurnState.Pat;
            CurrentGameState = GameState.Finished;
        }
    }
    private void MakeCastling(Move move)
    {
        int suitableRookXPosition = move.FinalPosition.x == 2 ? suitableRookXPosition = 0 : suitableRookXPosition = 7;
        var suitableRook = FiguresOnBoard.FirstOrDefault(figure => figure.Data.kind == Kind.Rook
                           && figure.Data.isWhite == IsWhiteTurn && figure.Data.position == new Vector2Int(suitableRookXPosition, move.CurrentFigure.Data.position.y));
        int rookOffset = suitableRookXPosition == 0 ? -1 : 1;
        suitableRook.Data.position = new Vector2Int(move.CurrentFigure.Data.position.x + rookOffset, move.CurrentFigure.Data.position.y);
        suitableRook.Data.turnCount++;
        suitableRook.transform.position = new Vector3(move.CurrentFigure.Data.position.x + rookOffset, 0, move.CurrentFigure.Data.position.y);
    }
    private void GenerateFigure(GameObject figurePrefab, FigureData data)
    {
        var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
        FiguresOnBoard.Add(figureGameObject.GetComponent<Figure>());
    }
    public bool IsAbleToMove(Figure figureToMove,List<Figure> figuresOnBoard, Vector2Int previousMoveFinalPosition, Vector2Int finalPosition, TurnState currentTurnState)
    {
        if (finalPosition.x < 0 || finalPosition.x > 7 || finalPosition.y < 0 || finalPosition.y > 7)
            return false;
        if (figureToMove.Data.position == finalPosition)
            return false;
        var figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == finalPosition);
        var delta = new Vector2Int(Mathf.Abs(finalPosition.x - figureToMove.Data.position.x), Mathf.Abs(finalPosition.y - figureToMove.Data.position.y));
        bool canMove = false;
        switch (figureToMove.Data.kind)
        {
            case Kind.Pawn:
                if (figureToMove.Data.isWhite && finalPosition.y > figureToMove.Data.position.y || !figureToMove.Data.isWhite && finalPosition.y < figureToMove.Data.position.y)
                {
                    if (figureToCapture == null)
                    {
                        if (figureToMove.Data.turnCount == 0)
                            if (delta.x == 0 && delta.y == 2)
                                canMove = true;
                        if (delta.x == 0 && delta.y == 1)
                            canMove = true;
                        if ((finalPosition.y == 5 || finalPosition.y == 2) && delta == Vector2Int.one)
                        {
                            int pawnPassageYLocation = finalPosition.y == 5 ? pawnPassageYLocation = 4 : pawnPassageYLocation = 3;
                            figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == new Vector2Int(finalPosition.x, pawnPassageYLocation)
                            && figureToMove.Data.isWhite != figure.Data.isWhite && figure.Data.kind == Kind.Pawn && figure.Data.turnCount == 1);
                            if (figureToCapture != null)
                                if (figureToCapture.Data.position == previousMoveFinalPosition)
                                    canMove = true;
                        }
                    }
                    else
                    {
                        if (delta == Vector2Int.one && figureToCapture.Data.isWhite != figureToMove.Data.isWhite)
                            canMove = true;
                    }
                }
                break;
            case Kind.Rook:
                canMove = CanMoveInConcrectDirections(figureToMove,figuresOnBoard, figureToCapture, finalPosition, Figure.RookDirections);
                break;
            case Kind.Knight:
                if ((delta.x == 1 && delta.y == 2) || (delta.x == 2 && delta.y == 1))
                    if (figureToCapture == null || figureToCapture.Data.isWhite != figureToMove.Data.isWhite)
                        canMove = true;
                break;
            case Kind.Bishop:
                canMove = CanMoveInConcrectDirections(figureToMove,figuresOnBoard, figureToCapture, finalPosition, Figure.BishopDirections);
                break;
            case Kind.Queen:
                canMove = CanMoveInConcrectDirections(figureToMove,figuresOnBoard, figureToCapture, finalPosition, Figure.AllDirections);
                break;
            case Kind.King:
                if (delta.x == 2 && delta.y == 0 && figureToMove.Data.turnCount == 0 && currentTurnState != TurnState.Check)
                {
                    if (finalPosition.x == 2 || finalPosition.x == 6 && figureToCapture == null)
                    {
                        int suitableRookXPosition = finalPosition.x == 2 ? suitableRookXPosition = 0 : suitableRookXPosition = 7;
                        var suitableRook = figuresOnBoard.FirstOrDefault(figure => figure.Data.kind == Kind.Rook
                                                         && figure.Data.isWhite == figureToMove.Data.isWhite && figure.Data.position == new Vector2Int(suitableRookXPosition, figureToMove.Data.position.y));
                        if (suitableRook != null && suitableRook.Data.turnCount == 0)
                            canMove = CanMoveInConcrectDirections(figureToMove,figuresOnBoard, figureToCapture, suitableRook.Data.position, Figure.RookDirections);
                    }
                }
                else if (figureToCapture == null || figureToCapture.Data.isWhite != figureToMove.Data.isWhite)
                    canMove = Figure.AllDirections.Contains(delta);
                break;
        }
        return canMove;
    }
    private bool CanMoveInConcrectDirections(Figure figureToMove,List<Figure> figuresOnBoard, Figure figureToCapture, Vector2Int finalPosition, Vector2Int[] allPossibleDirections)
    {
        Vector2Int[] figuresPositions = figuresOnBoard.Select(figure => figure.Data.position).ToArray();
        if (figureToCapture != null && figureToCapture.Data.isWhite == figureToMove.Data.isWhite)
            return false;
        var initialPosition = figureToMove.Data.position;
        var direction = ((Vector2)finalPosition - initialPosition).normalized;
        if (direction.x != 0 && direction.y != 0 && Mathf.Abs(direction.x) != Mathf.Abs(direction.y))
            return false;
        int directionalStepX = direction.x == 0 ? 0 : (int)(direction.x / Mathf.Abs(direction.x));
        int directionalStepY = direction.y == 0 ? 0 : (int)(direction.y / Mathf.Abs(direction.y));
        var directionalStep = new Vector2Int(directionalStepX, directionalStepY);
        if (allPossibleDirections.Contains(directionalStep))
        {
            initialPosition += directionalStep;
            while (initialPosition != finalPosition)
            {
                if (figuresPositions.Contains(initialPosition))
                    return false;
                initialPosition += directionalStep;
            }
            return true;
        }
        return false;
    }
    //Method for UI that calls when pawn achieves the end of the board 
    public void TransformPawnToNewFigure(string enumName)
    {
        Figure pawnInTheEnd = FiguresOnBoard.Find(figure => figure.Data.position == PreviousMoveFinalPosition);
        FigureData backUpData = pawnInTheEnd.Data;
        Enum.TryParse(enumName, out Kind figureKind);
        backUpData.kind = figureKind;
        FiguresOnBoard.Remove(pawnInTheEnd);
        Destroy(pawnInTheEnd.gameObject);
        GenerateFigure(modelMatcher.KindModelPairs[Tuple.Create(backUpData.kind, backUpData.isWhite)], backUpData);
        OnPause = false;
    }
}