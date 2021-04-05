using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveLoader:MonoBehaviour
{
    public BoardState LoadState(string path)
    {
        string validpath = Path.Combine(Application.dataPath, path);
        using (StreamReader reader = new StreamReader(validpath))
        {
            string json = reader.ReadToEnd();
            BoardState boardState = JsonUtility.FromJson<BoardState>(json);
            return boardState;
        }
    }
    #region Методы для UI
    public void Save()
    {
        BoardState boardState;
        Board board = FindObjectOfType<Board>();
        boardState.figuresData = FindObjectsOfType<Figure>().Select(figure => figure.Data).ToArray();
        boardState.isWhiteTurn = board.IsWhiteTurn;
        string path = Path.Combine(Application.dataPath, "Save.json");
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string json = JsonUtility.ToJson(boardState);
            streamWriter.Write(json);
        }
    }
    public void NewGame()
    {
        Board.CurrentGameState = GameState.NotStarted;
        SceneManager.LoadScene("Game");
    }
    public void LoadGame()
    {
        Board.CurrentGameState = GameState.Continues;
        SceneManager.LoadScene("Game");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
