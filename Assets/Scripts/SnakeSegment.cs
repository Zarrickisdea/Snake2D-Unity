using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Snake snake = collision.gameObject.GetComponent<Snake>();
        if (snake != null)
        {
            // Snake collided with itself
            snake.BurnSnake();
        }
    }
}
