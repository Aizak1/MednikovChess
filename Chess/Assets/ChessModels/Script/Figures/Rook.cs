using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Figure
{
    public override bool IsAbleToMove(Figure figureToCapture, Vector2Int gridPoint)
    {
        return true;
    }
}
