using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Figure
{
    public override bool IsAbleToMove(Figure figureToCapture, Vector2Int gridPoint)
    {
        var delta = CalculateDelta(Data.position, gridPoint);
        if((delta.x == 1 && delta.y == 2) || (delta.x == 2 && delta.y == 1))
        {
            if (figureToCapture == null)
                return true;
            else if (figureToCapture.Data.isWhite != Data.isWhite)
                return true;
        }
                
        return false;
    }
}
