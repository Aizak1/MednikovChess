using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    public BoardState initialState;
    
    private void Start()
    {
       string initialStatePath = "Initial.json";
       initialState = saveLoader.Load(initialStatePath);
       for (int i = 0; i < initialState.figureObjects.Length; i++)
            GenetateFigure(initialState.figureObjects[i], initialState.figurePositions[i]);
       
    }

    private void GenetateFigure(GameObject figurePrefab,Vector3 position)
    {
        Instantiate(figurePrefab, position, Quaternion.identity, transform);
    }
}

