using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rook : Figure
{
    public override bool IsAbleToMove(Figure figureToCapture, Vector2Int gridPoint)
    {
        return DirectionalFigureIsAbleToMove(figureToCapture, gridPoint, rookDirections);
    }
}
