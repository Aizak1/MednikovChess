using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Figure
{
    public override bool IsAbleToMove(Figure figureToCapture, Vector2Int finalPosition)
    {
        if (figureToCapture != null && figureToCapture.Data.isWhite == Data.isWhite)
            return false;
        List<Vector2Int> directions = new List<Vector2Int>(bishopDirections);
        directions.AddRange(rookDirections);
        return directions.Contains(CalculateDelta(Data.position, finalPosition));
        

    }
}
