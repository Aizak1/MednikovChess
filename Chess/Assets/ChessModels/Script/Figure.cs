using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    public FigureData Data;

    public List<Vector2Int> GetAllMoves(Figure figureToCapture)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                Vector2Int finalPosition = new Vector2Int(x, y);
                Vector2Int delta = CalculateDelta(Data.position, finalPosition);
                switch (Data.kind)
                {
                    case Kind.Pawn:
                       if(Data.isWhite && finalPosition.y >  Data.position.y || !Data.isWhite && finalPosition.y < Data.position.y)
                        {
                            if (figureToCapture == null)
                            {
                                if (Data.isWhite && Data.position.y == 1 || !Data.isWhite && Data.position.y == 6)
                                    if (delta.x == 0 && delta.y == 2)
                                        positions.Add(finalPosition);
                                if (delta.x == 0 && delta.y == 1)
                                    positions.Add(finalPosition);
                            }
                            else
                            {
                                if (delta == Vector2Int.one && figureToCapture.Data.isWhite != Data.isWhite)
                                    positions.Add(finalPosition);
                            }
                        }
                        break;
                    case Kind.Rook:
                        if(delta.x != delta.y)
                            positions.Add(finalPosition);
                        break;
                    case Kind.Knight:
                        break;
                    case Kind.Bishop:
                        break;
                    case Kind.Queen:
                        break;
                    case Kind.King:
                        break;
                    default:
                        break;
                }
            }
        }
       
        return positions;
    }
    private Vector2Int CalculateDelta(Vector2Int initialPosition,Vector2Int finalPosition)
    {
        return new Vector2Int(Mathf.Abs(finalPosition.x - initialPosition.x), Mathf.Abs(finalPosition.y - initialPosition.y));
    }
   
}
