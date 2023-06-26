using UnityEngine;

public class Walls : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Snake snake = collision.GetComponent<Snake>();

        if (snake != null)
        {
            snake.BurnSnake();
        }
    }
}
