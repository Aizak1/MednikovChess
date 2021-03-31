
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
            GenetateFigure(initialModels[i], initialState.figuresData[i]);
        }
    }
    private void Update()
    {
        SelectTile();
    }

    private void GenetateFigure(GameObject figurePrefab, FigureData data)
    {
       var figureGameObject = Instantiate(figurePrefab, new Vector3(data.position.x, 0, data.position.y), Quaternion.identity);
        figureGameObject.GetComponent<Figure>().Data = data;
    }

    private void SelectTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Board")))
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
