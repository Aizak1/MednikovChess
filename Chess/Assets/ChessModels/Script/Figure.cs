using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Figure : MonoBehaviour
{
    public FigureData Data;
    private readonly static Vector2Int[] rookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    private readonly static Vector2Int[] bishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
    private readonly static Vector2Int[] allDirections = rookDirections.Union(bishopDirections).ToArray();
    public bool IsAbleToMove(List<Figure> figuresOnBoard,Vector2Int previousMoveFinalPosition,Vector2Int finalPosition)
    {
        if (finalPosition.x < 0 || finalPosition.x > 7 || finalPosition.y < 0 || finalPosition.y > 7)
            return false;
        if (Data.position == finalPosition)
            return false;
        var figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == finalPosition);
        var delta = new Vector2Int(Mathf.Abs(finalPosition.x - Data.position.x), Mathf.Abs(finalPosition.y - Data.position.y));
        bool canMove = false;
        switch (Data.kind)
        {
            case Kind.Pawn:
                if (Data.isWhite && finalPosition.y > Data.position.y || !Data.isWhite && finalPosition.y < Data.position.y)
                {
                    if (figureToCapture == null)
                    {
                        if (Data.turnCount == 0)
                            if (delta.x == 0 && delta.y == 2)
                                canMove = true;
                        if (delta.x == 0 && delta.y == 1)
                           canMove = true;
                        if((finalPosition.y == 5 || finalPosition.y == 2) && delta == Vector2Int.one)
                        {
                            int pawnPassageYLocation = finalPosition.y == 5 ? pawnPassageYLocation = 4 : pawnPassageYLocation = 3;
                            figureToCapture = figuresOnBoard.FirstOrDefault(figure => figure.Data.position == new Vector2Int(finalPosition.x, pawnPassageYLocation)
                            && Data.isWhite != figure.Data.isWhite && figure.Data.kind == Kind.Pawn && figure.Data.turnCount == 1);
                            if (figureToCapture != null)
                                if(figureToCapture.Data.position == previousMoveFinalPosition)
                                    canMove = true;
                        }
                    }
                    else
                    {
                        if (delta == Vector2Int.one && figureToCapture.Data.isWhite != Data.isWhite)
                            canMove = true;
                    }
                }
                break;
            case Kind.Rook:
                canMove =  IsDirectionalFigureAbleToMove(figuresOnBoard, figureToCapture, finalPosition, rookDirections);
                break;
            case Kind.Knight:
                if ((delta.x == 1 && delta.y == 2) || (delta.x == 2 && delta.y == 1))
                    if (figureToCapture == null || figureToCapture.Data.isWhite != Data.isWhite)
                        canMove =  true;
                break;
            case Kind.Bishop:
                canMove =  IsDirectionalFigureAbleToMove(figuresOnBoard, figureToCapture, finalPosition, bishopDirections);
                break;
            case Kind.Queen:
                canMove =  IsDirectionalFigureAbleToMove(figuresOnBoard, figureToCapture, finalPosition, allDirections);
                break;
            case Kind.King:
                if (delta.x == 2 && Data.turnCount == 0 && Board.CurrentTurnState != TurnState.Check)
                {
                    if (finalPosition.x == 2 || finalPosition.x == 6 && figureToCapture == null)
                    {
                        int suitableRookXPosition = finalPosition.x == 2 ? suitableRookXPosition = 0 : suitableRookXPosition = 7;
                        var suitableRook = figuresOnBoard.FirstOrDefault(figure => figure.Data.kind == Kind.Rook 
                                                         && figure.Data.isWhite == Data.isWhite && figure.Data.position == new Vector2Int(suitableRookXPosition, Data.position.y));
                        if(suitableRook!=null && suitableRook.Data.turnCount == 0)
                            canMove = IsDirectionalFigureAbleToMove(figuresOnBoard, figureToCapture, suitableRook.Data.position, rookDirections);
                    } 
                }
                else if (figureToCapture == null || figureToCapture.Data.isWhite != Data.isWhite)
                        canMove = allDirections.Contains(delta);
                break;
        }
        return canMove;
    }
    private bool IsDirectionalFigureAbleToMove(List<Figure>figuresOnBoard,Figure figureToCapture, Vector2Int finalPosition,Vector2Int[] allPossibleDirections)
    {
        Vector2Int[] figuresPositions = figuresOnBoard.Select(figure => figure.Data.position).ToArray();
        if (figureToCapture != null && figureToCapture.Data.isWhite == Data.isWhite)
            return false;
        var initialPosition = Data.position;
        var direction = ((Vector2)finalPosition - initialPosition).normalized;
        if(direction.x != 0 && direction.y != 0 && Mathf.Abs(direction.x) != Mathf.Abs(direction.y))
            return false;
        int directionalStepX = direction.x == 0 ? 0 : (int)(direction.x / Mathf.Abs(direction.x));
        int directionalStepY = direction.y == 0 ? 0 : (int)(direction.y / Mathf.Abs(direction.y));
        var directionalStep = new Vector2Int(directionalStepX,directionalStepY);
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
}
