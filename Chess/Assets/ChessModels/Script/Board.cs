using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject[] initialModels;
    public BoardState initialState;
    
    private void Start()
    {
        initialState = saveLoader.Load();
        for (int i = 0; i < initialState.figures.Length; i++)
        {
            GenetateFigure(initialModels[i], initialState.figures[i].position);
        }
    }

    private void GenetateFigure(GameObject figurePrefab,Vector2Int position)
    {
        Instantiate(figurePrefab, new Vector3(position.x,0,position.y), Quaternion.identity, transform);
    }

    
}

