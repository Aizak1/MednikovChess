using UnityEngine;
[System.Serializable]
public class BoardState
{
  public GameObject[] figureObjects;
  public Vector3[] figurePositions;
    public BoardState(GameObject[]figureObjects,Vector3[] figurePositions)
    {
        this.figureObjects = figureObjects;
        this.figurePositions = figurePositions;
    }
}


