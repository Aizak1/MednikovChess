using UnityEngine;
[System.Serializable]
public struct BoardState
{
    public FigureData[] figuresData;
    public bool isWhiteTurn;
    public TurnState currentTurnState;
    public BoardState(FigureData[] figuresData, bool isWhiteTurn,TurnState currentTurnState)
    {
        this.figuresData = figuresData;
        this.isWhiteTurn = isWhiteTurn;
        this.currentTurnState = currentTurnState;
    }
}


