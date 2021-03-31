
using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject[] initialModels;
    public BoardState initialState;
    private Figure selectedFigure;
    private bool isWhiteTurn;

    private void Start()
    {
        isWhiteTurn = true;
        initialState = saveLoader.Load();
        for (int i = 0; i < initialState.figuresData.Length; i++)
        {
            GenetateFigure(initialModels[i], initialState.figuresData[i]);
        }
    }
    private void Update()
    {
       SelectTile(out Vector2Int gridPoint);
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedFigure != null)
                TryMakeTurn(gridPoint);
        }
                  
    }

    private void TryMakeTurn(Vector2Int gridPoint)
    {
        if (selectedFigure.Data.position == gridPoint)
        {
            selectedFigure.transform.position = new Vector3(gridPoint.x, 0, gridPoint.y);
            selectedFigure = null;
            return;
        }
        selectedFigure.Data.position = gridPoint;
        selectedFigure.transform.position = new Vector3(gridPoint.x, 0, gridPoint.y);
        selectedFigure = null;
        isWhiteTurn = !isWhiteTurn;
    }

    private void GenetateFigure(GameObject figurePrefab, FigureData data)
    {
       var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
    }

    private void SelectTile(out Vector2Int gridPoint)
    {
        gridPoint = Vector2Int.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.589f, 0.45f);
             gridPoint = new Vector2Int((int)(hit.point.x + cellOffset.x), (int)(hit.point.z + cellOffset.y));
            if (Input.GetMouseButtonDown(0))
            {
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                if (figure != null && figure.Data.isWhite == isWhiteTurn)
                 selectedFigure = hit.transform.gameObject.GetComponent<Figure>();
            }
            if (selectedFigure != null)
                selectedFigure.transform.position = hit.point + Vector3.up;

        }

    }
   


}
