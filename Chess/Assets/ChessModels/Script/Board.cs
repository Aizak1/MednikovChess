using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject[] initialModels;
    public BoardState initialState;
    private Figure selectedFigure;
    
    private void Start()
    {
        initialState = saveLoader.Load();
        for (int i = 0; i < initialState.figuresData.Length; i++)
        {
           initialModels[i].GetComponent<Figure>().Data = initialState.figuresData[i];
           GenetateFigure(initialModels[i], initialState.figuresData[i].position);
        }
    }
    private void Update()
    {
        SelectTile();
    }

    private void GenetateFigure(GameObject figurePrefab,Vector2Int position)
    {
        Instantiate(figurePrefab, new Vector3(position.x,0,position.y), Quaternion.identity);
    }

    private void SelectTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.589f, 0.45f);
            Vector2Int gridPoint = new Vector2Int((int)(hit.point.x + cellOffset.x), (int)(hit.point.z + cellOffset.y));
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.GetComponent<Figure>())
                {
                    float optimalFigureYOffset = 0.3f;
                    if (selectedFigure == null)
                    {
                        selectedFigure = hit.transform.gameObject.GetComponent<Figure>();
                        selectedFigure.transform.position = new Vector3(gridPoint.x, optimalFigureYOffset, gridPoint.y);
                    }
                    else
                    {
                        selectedFigure.transform.position = new Vector3(selectedFigure.transform.position.x, 0, selectedFigure.transform.position.z);
                        selectedFigure = hit.transform.gameObject.GetComponent<Figure>();
                        selectedFigure.transform.position = new Vector3(gridPoint.x, optimalFigureYOffset, gridPoint.y);
                    }
                }
                
            }
            
        }
        
    }

    
}

