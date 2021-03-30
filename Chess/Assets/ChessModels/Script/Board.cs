using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    public BoardState initialState;
    
    private void Start()
    {
       initialState = saveLoader.Load();
       for (int i = 0; i < initialState.figureObjects.Length; i++)
            GenetateFigure(initialState.figureObjects[i], initialState.figurePositions[i]);
    }

    private void GenetateFigure(GameObject figurePrefab,Vector3 position)
    {
        Instantiate(figurePrefab, position, Quaternion.identity, transform);
    }

    
}

