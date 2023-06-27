
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        GameHandler.NumberOfSnakes = 0;
        Debug.Log(GameHandler.NumberOfSnakes);
    }
}
