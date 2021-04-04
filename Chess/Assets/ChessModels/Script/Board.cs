using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public enum GameState
{
    NotStarted,
    Continues,
    Finished
}
public class Board : MonoBehaviour
{
    [SerializeField] private SaveLoader saveLoader;
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private ModelMathcer modelMatcher;
    [SerializeField] private BoardState initialState;
    private Figure selectedFigure;
    public bool IsWhiteTurn { get; private set; }
    public static GameState CurrentGameState { get; set; }
    private void Start()
    {
        if (CurrentGameState == GameState.NotStarted)
            initialState = saveLoader.LoadState("Initial.json");
        else if (CurrentGameState == GameState.Continues)
            initialState = saveLoader.LoadState("Save.json");
        for (int i = 0; i < initialState.figuresData.Length; i++)
            GenetateFigure(modelMatcher.KindModelPairs[Tuple.Create(initialState.figuresData[i].kind, initialState.figuresData[i].isWhite)], initialState.figuresData[i]);
        IsWhiteTurn = initialState.isWhiteTurn;
    }
    private void Update()
    {
        Vector2Int mouseDownPosition = Vector2Int.zero - Vector2Int.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Board")))
        {
            Vector2 cellOffset = new Vector2(0.565f, 0.45f);
            mouseDownPosition = new Vector2Int((int)(hit.point.x + cellOffset.x), (int)(hit.point.z + cellOffset.y));
            tileHighlighter.SetActive(true);
            Vector3 tileHighlighterOffset = new Vector3(0.04f, 0.126f, -0.03f);
            tileHighlighter.transform.position = new Vector3(mouseDownPosition.x + tileHighlighterOffset.x, tileHighlighterOffset.y, mouseDownPosition.y + tileHighlighterOffset.z);
            if (Input.GetMouseButtonDown(0))
            {
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                if (figure != null && figure.Data.isWhite == IsWhiteTurn)
                    selectedFigure = figure;
            }
            if (selectedFigure != null)
                selectedFigure.transform.position = hit.point + Vector3.up;
        }
        if (Input.GetMouseButtonUp(0))
            if (selectedFigure != null)
                TryMakeTurn(mouseDownPosition);
    }
    private void TryMakeTurn(Vector2Int finalPosition)
    {
        Vector2Int initialPosition = selectedFigure.Data.position;
        var figuresOnBoard = FindObjectsOfType<Figure>();
        if (!selectedFigure.IsAbleToMove(figuresOnBoard, finalPosition, out Figure figureToCapture))
        {
            selectedFigure.transform.position = new Vector3(initialPosition.x, 0, initialPosition.y);
            selectedFigure = null;
            return;
        }
        if (figureToCapture != null)
            Destroy(figureToCapture.gameObject);
        selectedFigure.Data.position = finalPosition;
        selectedFigure.transform.position = new Vector3(finalPosition.x, 0, finalPosition.y);
        selectedFigure = null;
        IsWhiteTurn = !IsWhiteTurn;
    }
    private void GenetateFigure(GameObject figurePrefab, FigureData data)
    {
        var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
    }
}