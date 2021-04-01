using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Figure
{
    public override bool IsAbleToMove(Figure[] figuresOnBoard, Vector2Int gridPoint)
    {
        return true;
    }
}
