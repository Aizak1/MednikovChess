using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Figure : MonoBehaviour
{
    public FigureData Data;
    public readonly static Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    public readonly static Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
    public readonly static Vector2Int[] AllDirections = RookDirections.Union(BishopDirections).ToArray();
}