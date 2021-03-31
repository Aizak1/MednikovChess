using UnityEngine;
[System.Serializable]
public struct BoardState
{
    public FigureData[] figuresData;
    public BoardState(FigureData[]figures)
    {
        this.figuresData = figures;
    }
}


