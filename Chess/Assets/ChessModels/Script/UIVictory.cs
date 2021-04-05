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
            if (board.IsWhiteTurn)
                winText.text = "Black Wins";
            else
                winText.text = "White Wins";
        }
    }
}
