using UnityEngine;
[System.Serializable]
public class BoardState
{
    public Figure[] figures;
    public BoardState(Figure[]figures)
    {
        this.figures = figures;
    }
}


