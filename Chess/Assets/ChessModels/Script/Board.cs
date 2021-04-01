using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject[] initialModels;
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private BoardState initialState;
    private Figure selectedFigure;
    private bool isWhiteTurn;
    private Vector3 tileHighlighterOffset = new Vector3(0.04f, 0.126f, -0.03f);

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
        SelectTile(out Vector2Int mouseDownPosition);
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedFigure != null)
                TryMakeTurn(mouseDownPosition);
        }

    }

    private void TryMakeTurn(Vector2Int finalPosition)
    {
        Vector2Int initialPosition = selectedFigure.Data.position;
        if(finalPosition.x<0 || finalPosition.x>7 || finalPosition.y<0 || finalPosition.y > 7)
        {
            Deselect(initialPosition);
            return;
        }
        if (initialPosition == finalPosition)
        {
            Deselect(initialPosition);
            return;
        }
        var figuresOnBoard = FindObjectsOfType<Figure>();
        if (!selectedFigure.IsAbleToMove(figuresOnBoard,finalPosition))
        {
            Deselect(initialPosition);
            return;
        }  
        var figureOnPoint = figuresOnBoard.FirstOrDefault(x => x.Data.position == finalPosition);
        if (figureOnPoint != null)
            Destroy(figureOnPoint.gameObject);
        selectedFigure.Data.position = finalPosition;
        selectedFigure.transform.position = new Vector3(finalPosition.x, 0, finalPosition.y);
        selectedFigure = null;
        isWhiteTurn = !isWhiteTurn;
    }

    private void Deselect(Vector2Int initialPosition)
    {
        selectedFigure.transform.position = new Vector3(initialPosition.x, 0, initialPosition.y);
        selectedFigure = null;
    }

    private void GenetateFigure(GameObject figurePrefab, FigureData data)
    {
        var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
    }

    private void SelectTile(out Vector2Int mouseDownPosition)
    {
        mouseDownPosition = Vector2Int.zero-Vector2Int.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.565f, 0.45f);
            mouseDownPosition = new Vector2Int((int)(hit.point.x+cellOffset.x), (int)(hit.point.z+cellOffset.y));
            tileHighlighter.SetActive(true);
            tileHighlighter.transform.position = new Vector3(mouseDownPosition.x+tileHighlighterOffset.x, tileHighlighterOffset.y, mouseDownPosition.y+tileHighlighterOffset.z);
            if (Input.GetMouseButtonDown(0))
            {
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                if (figure != null && figure.Data.isWhite == isWhiteTurn)
                    selectedFigure = figure;
            }
            if (selectedFigure != null)
                selectedFigure.transform.position = hit.point + Vector3.up;

        }

    }



}