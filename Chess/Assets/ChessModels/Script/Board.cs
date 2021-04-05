using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public enum GameState
{
    NotStarted,
    Continues,
    Finished
}
public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private ModelMathcer modelMatcher;
    [SerializeField] private BoardState initialState;
    private Figure selectedFigure;
    private List<Figure> figuresOnBoard;
    private List<Move> currentTurnMoves;
    public bool IsWhiteTurn { get; private set; }
    public static GameState CurrentGameState { get; set; }
    private void Start()
    {
        if (CurrentGameState == GameState.NotStarted)
            initialState = saveLoader.LoadState("Initial.json");
        else if (CurrentGameState == GameState.Continues)
            initialState = saveLoader.LoadState("Save.json");
        for (int i = 0; i < initialState.figuresData.Length; i++)
            GenetateFigure(modelMatcher.KindModelPairs[Tuple.Create(initialState.figuresData[i].kind, initialState.figuresData[i].isWhite)], initialState.figuresData[i]);
        IsWhiteTurn = initialState.isWhiteTurn;
        figuresOnBoard = FindObjectsOfType<Figure>().ToList();
    }
    private void Update()
    {
        Vector2Int mouseDownPosition = Vector2Int.zero - Vector2Int.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.565f, 0.45f);
            mouseDownPosition = new Vector2Int((int)(hit.point.x + cellOffset.x), (int)(hit.point.z + cellOffset.y));
            tileHighlighter.SetActive(true);
            Vector3 tileHighlighterOffset = new Vector3(0.04f, 0.126f, -0.03f);
            tileHighlighter.transform.position = new Vector3(mouseDownPosition.x + tileHighlighterOffset.x, tileHighlighterOffset.y, mouseDownPosition.y + tileHighlighterOffset.z);
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < figuresOnBoard.Count; i++)
                {
                    if (figuresOnBoard[i] == null)
                        figuresOnBoard.RemoveAt(i);
                        
                }
                currentTurnMoves = GetAllMyMoves(figuresOnBoard);
                if(currentTurnMoves.Count == 0)
                {
                    CurrentGameState = GameState.Finished;
                    return;
                }
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                bool figureIsAbleToMove = currentTurnMoves.Select(move => move.CurrentFigure).ToList().Contains(figure);
                if (figure != null && figure.Data.isWhite == IsWhiteTurn && figureIsAbleToMove)
                    selectedFigure = figure;
            }
            Vector3 optimalHightForSelectedFigure = 2 * Vector3.up;
            if (selectedFigure != null) 
                selectedFigure.transform.position = hit.point + optimalHightForSelectedFigure;
        }
        if (Input.GetMouseButtonUp(0))
            if (selectedFigure != null)
                TryMakeTurn(new Move(selectedFigure,mouseDownPosition));
    }
    private List<Move> GetAllMyMoves(List<Figure> figuresOnBoard)
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
                    if(figure.IsAbleToMove(figuresOnBoard,finalPosition))
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
                        var opponentTurnFigures = boardCopy.Where(figure =>  figure.Data.isWhite != IsWhiteTurn).ToArray();
                        var currentKing = boardCopy.FirstOrDefault(figure => figure.Data.kind == Kind.King && figure.Data.isWhite == IsWhiteTurn);
                        bool canEatKing = false;
                        foreach (var opponentFigure in opponentTurnFigures)
                        {
                            if (opponentFigure.IsAbleToMove(boardCopy, currentKing.Data.position))
                                canEatKing = true;
                        }
                        if (!canEatKing)
                            possibleMoves.Add(new Move(figure, finalPosition));
                        boardCopy[indexOfFigure].Data = figureDataBackUp;
                    }
                }
            }
        }
        return possibleMoves;
    }
    private void TryMakeTurn(Move move)
    {
        Vector2Int initialPosition = move.CurrentFigure.Data.position;
        var possibleMove = currentTurnMoves.FirstOrDefault(validMove => validMove.CurrentFigure == move.CurrentFigure && validMove.FinalPosition == move.FinalPosition);
        if (possibleMove == null)
        {
            move.CurrentFigure.transform.position = new Vector3(initialPosition.x, 0, initialPosition.y);
            selectedFigure = null;
            return;
        }
        var figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == move.FinalPosition);
        if (figureToCapture != null)
            Destroy(figureToCapture.gameObject);
        move.CurrentFigure.Data.position = move.FinalPosition;
        move.CurrentFigure.transform.position = new Vector3(move.FinalPosition.x, 0, move.FinalPosition.y);
        selectedFigure = null;
        figuresOnBoard = FindObjectsOfType<Figure>().ToList();
        IsWhiteTurn = !IsWhiteTurn;
    }
    private void GenetateFigure(GameObject figurePrefab, FigureData data)
    {
        var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
    }
}