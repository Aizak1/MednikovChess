using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private Canvas saveLoadCanvas;
    [SerializeField] private Canvas selectionCanvas;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private Board board;

    void Update()
    {
        if (Board.CurrentGameState == GameState.Finished)
        {
            saveLoadCanvas.enabled = false;
            victoryCanvas.enabled = true;
            if (board.CurrentTurnState == TurnState.CheckAndMate)
                if (board.IsWhiteTurn)
                    endText.text = $"Black Wins";
                else
                    endText.text = $"White Wins";
            else if (board.CurrentTurnState == TurnState.Pat)
                endText.text = "Draw Match";
            
        }
        if (board.OnPause)
        {
            saveLoadCanvas.enabled = false;
            selectionCanvas.enabled = true;
        }
        else if (Board.CurrentGameState != GameState.Finished)
        {
            saveLoadCanvas.enabled = true;
            selectionCanvas.enabled = false;
        }
       
    }
   
}
