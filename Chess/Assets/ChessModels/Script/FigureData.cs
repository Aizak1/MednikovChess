using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
public struct FigureData
{
   public Vector2Int position;
   public  bool isWhite;
   public  Kind kind;
   public FigureData(Vector2Int position, bool isWhite, Kind kind)
   {
     this.position = position;
     this.isWhite = isWhite;
     this.kind = kind;
   }
    
}
