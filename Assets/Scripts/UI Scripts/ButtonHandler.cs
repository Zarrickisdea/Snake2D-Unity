using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public int snakesToSpawn;

    public void SetNumberOfSnakes()
    {
        GameHandler.NumberOfSnakes = snakesToSpawn;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

