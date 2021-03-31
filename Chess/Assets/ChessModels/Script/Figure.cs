using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Kind
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King,
}
[System.Serializable]
public class Figure
{
   public Vector2Int position;
   public bool isWhite;
   public Kind kind;

    public Figure(Vector2Int position, bool isWhite, Kind kind,GameObject model)
    {
        this.position = position;
        this.isWhite = isWhite;
        this.kind = kind;
    }
}
