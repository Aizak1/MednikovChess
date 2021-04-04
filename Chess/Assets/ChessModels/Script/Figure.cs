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
    public static Vector2Int[] AllDirections => allDirections;
    public bool IsAbleToMove(Figure[] board,Vector2Int finalPosition,out Figure figureToCapture)
    {
        figureToCapture = board.FirstOrDefault(figure => figure.Data.position == finalPosition);
        if (finalPosition.x < 0 || finalPosition.x > 7 || finalPosition.y < 0 || finalPosition.y > 7)
            return false;
        if (Data.position == finalPosition)
            return false;
        var delta = new Vector2Int(Mathf.Abs(finalPosition.x - Data.position.x), Mathf.Abs(finalPosition.y - Data.position.y));
        bool canMove = false;
        switch (Data.kind)
        {
            case Kind.Pawn:
                if (Data.isWhite && finalPosition.y > Data.position.y || !Data.isWhite && finalPosition.y < Data.position.y)
                {
                    if (figureToCapture == null)
                    {
                        if (Data.isWhite && Data.position.y == 1 || !Data.isWhite && Data.position.y == 6 )
                            if (delta.x == 0 && delta.y == 2)
                                canMove = true;
                        if (delta.x == 0 && delta.y == 1)
                            canMove = true;
                    }
                    else
                    {
                        if (delta == Vector2Int.one && figureToCapture.Data.isWhite != Data.isWhite)
                            canMove = true;
                    }
                }
                break;
            case Kind.Rook:
                canMove =  IsDirectionalFigureAbleToMove(board, figureToCapture, finalPosition, rookDirections);
                break;
            case Kind.Knight:
                if ((delta.x == 1 && delta.y == 2) || (delta.x == 2 && delta.y == 1))
                    if (figureToCapture == null || figureToCapture.Data.isWhite != Data.isWhite)
                        canMove =  true;
                break;
            case Kind.Bishop:
                canMove =  IsDirectionalFigureAbleToMove(board, figureToCapture, finalPosition, bishopDirections);
                break;
            case Kind.Queen:
                canMove =  IsDirectionalFigureAbleToMove(board, figureToCapture, finalPosition, allDirections);
                break;
            case Kind.King:
                if (figureToCapture == null || figureToCapture.Data.isWhite != Data.isWhite)
                    canMove = allDirections.Contains(delta);
                break;
        }
        return canMove;
    }
    private bool IsDirectionalFigureAbleToMove(Figure[]board,Figure figureToCapture, Vector2Int finalPosition,Vector2Int[] allPossibleDirections)
    {
        Vector2Int[] figuresPositions = board.Select(figure => figure.Data.position).ToArray();
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
