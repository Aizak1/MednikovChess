using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Figure : MonoBehaviour
{
    public FigureData Data;

    protected Vector2Int[] rookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] bishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    public abstract bool IsAbleToMove(Figure figureToCapture,Vector2Int gridPoint);

    protected bool DirectionalFigureIsAbleToMove(Figure figureToCapture, Vector2Int gridPoint,Vector2Int[] allPossibleDirections)
    {
        Vector2Int[] figuresPositions = FindObjectsOfType<Figure>().Select(figure => figure.Data.position).ToArray();
        if (figureToCapture != null && figureToCapture.Data.isWhite == Data.isWhite)
            return false;
        var initialCoordinate = Data.position;
        var delta = (Vector2)(gridPoint - initialCoordinate);
        var directionalStep = new Vector2Int((int)delta.normalized.x, (int)delta.normalized.y);
        if (allPossibleDirections.Contains(directionalStep))
        {
            initialCoordinate += directionalStep;
            while (initialCoordinate != gridPoint)
            {
                if (figuresPositions.Contains(initialCoordinate))
                    return false;
                initialCoordinate += directionalStep;
            }
            return true;
        }

        return false;
    }
    
    protected Vector2Int CalculateDelta(Vector2Int initialPosition,Vector2Int finalPosition)
    {
        return new Vector2Int(Mathf.Abs(finalPosition.x - initialPosition.x), Mathf.Abs(finalPosition.y - initialPosition.y));
    }
   
}
