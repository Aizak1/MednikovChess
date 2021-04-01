using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pawn : Figure
{
    private bool firstTurn;
    public Pawn()
    {
        firstTurn = true;
    }
    public override bool IsAbleToMove(Figure[] figuresOnBoard,Vector2Int finalPosition)
    {
        var figureToCapture = figuresOnBoard.FirstOrDefault(x => x.Data.position == finalPosition);
        Vector2Int delta = CalculateDelta(Data.position, finalPosition);
        if (Data.isWhite && finalPosition.y > Data.position.y || !Data.isWhite && finalPosition.y < Data.position.y)
        {
            if (figureToCapture == null)
            {
                if (firstTurn)
                    if (delta.x == 0 && delta.y == 2)
                    {
                        firstTurn = false;
                        return true;
                    }
                        
                if (delta.x == 0 && delta.y == 1)
                {
                    firstTurn = false;
                    return true;
                }
                    
            }
            else
            {
                if (delta == Vector2Int.one && figureToCapture.Data.isWhite != Data.isWhite)
                    return true;
            }
        }
        return false;
    }
}
