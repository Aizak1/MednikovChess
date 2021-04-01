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

    public abstract bool IsAbleToMove(Figure[] figuresOnBoard,Vector2Int gridPoint);
    
    protected Vector2Int CalculateDelta(Vector2Int initialPosition,Vector2Int finalPosition)
    {
        return new Vector2Int(Mathf.Abs(finalPosition.x - initialPosition.x), Mathf.Abs(finalPosition.y - initialPosition.y));
    }
   
}
