using UnityEngine;

public class Move
{
    private Figure currentFigure;
    private Vector2Int finalPosition;
    public Figure CurrentFigure => currentFigure;
    public Vector2Int FinalPosition => finalPosition;
    public Move(Figure currentFigure, Vector2Int finalPosition)
    {
        this.currentFigure = currentFigure;
        this.finalPosition = finalPosition;
    }
}
