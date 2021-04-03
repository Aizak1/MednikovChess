using UnityEngine;
[System.Serializable]
public struct BoardState
{
    public FigureData[] figuresData;
    public bool isWhiteTurn;
    public BoardState(FigureData[] figuresData, bool isWhiteTurn)
    {
        this.figuresData = figuresData;
        this.isWhiteTurn = isWhiteTurn;
    }
}


