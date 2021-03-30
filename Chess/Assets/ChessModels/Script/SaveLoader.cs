using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoader:MonoBehaviour
{
    public void Save(BoardState boardState)
    {
        string path = Path.Combine(Application.dataPath, "Save.json");
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            string json = JsonUtility.ToJson(boardState);
            streamWriter.Write(json);
        }
    }
    public BoardState Load(string path = "Initial.json")
    {
        string validpath = Path.Combine(Application.dataPath, path);
        using (StreamReader reader = new StreamReader(validpath))
        {
            string json = reader.ReadToEnd();
            BoardState boardState = JsonUtility.FromJson<BoardState>(json);
            return boardState;
        }
    }
}
