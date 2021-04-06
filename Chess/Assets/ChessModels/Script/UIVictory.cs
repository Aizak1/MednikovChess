using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : MonoBehaviour
{
    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private Canvas saveLoadCanvas;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private Board board;
    
    void Update()
    {
        if(Board.CurrentGameState == GameState.Finished)
        {
            saveLoadCanvas.enabled = false;
            victoryCanvas.enabled = true;
            string kindOfend = "";
            if (Board.CurrentTurnState == TurnState.CheckAndMate)
                kindOfend = "(Mate)";
            else if (Board.CurrentTurnState == TurnState.Pat)
                kindOfend = "(Pat)";
            if (board.IsWhiteTurn)
                winText.text = $"Black Wins{kindOfend}";
            else
                winText.text = $"White Wins{kindOfend}";
        }
    }
}
