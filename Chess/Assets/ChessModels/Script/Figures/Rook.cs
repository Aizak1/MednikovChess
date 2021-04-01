using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rook : Figure
{
    public override bool IsAbleToMove(Figure figureToCapture, Vector2Int gridPoint)
    {
        Vector2Int[] figuresPositions = FindObjectsOfType<Figure>().Select(figure => figure.Data.position).ToArray();
        if (figureToCapture != null && figureToCapture.Data.isWhite == Data.isWhite)
            return false;
        var initialCoordinate = Data.position;
        var delta = (Vector2)(gridPoint - initialCoordinate);
        var directionalStep = new Vector2Int((int)delta.normalized.x, (int)delta.normalized.y);
        if (rookDirections.Contains(directionalStep))
        {
            initialCoordinate += directionalStep;
            while (initialCoordinate != gridPoint+directionalStep)
            {
                if (figuresPositions.Contains(initialCoordinate))
                    return false;
                initialCoordinate += directionalStep;
            }
            return true;
        }

        return false;
    }
}
