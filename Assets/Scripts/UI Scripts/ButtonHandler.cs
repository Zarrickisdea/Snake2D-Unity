using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private int snakesToSpawn;

    private void Awake()
    {
        AudioManager.Instance.PlayBGM(AudioManager.BackgroundSound.Menu);
    }
    public void SetNumberOfSnakes()
    {
        GameHandler.NumberOfSnakes = snakesToSpawn;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AudioManager.Instance.PlayBGM(AudioManager.BackgroundSound.Gameplay);
    }
}

