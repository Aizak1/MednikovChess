using UnityEngine;
[System.Serializable]
public struct BoardState
{
    public FigureData[] figuresData;
    public bool isWhiteTurn;
    public TurnState currentTurnState;
    public Vector2Int previousMoveFinalPosition;
    public BoardState(FigureData[] figuresData, bool isWhiteTurn,TurnState currentTurnState,Vector2Int previousMoveFinalPosition)
    {
        this.figuresData = figuresData;
        this.isWhiteTurn = isWhiteTurn;
        this.currentTurnState = currentTurnState;
        this.previousMoveFinalPosition = previousMoveFinalPosition;
    }
}


