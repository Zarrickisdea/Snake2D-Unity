using UnityEngine;

public class Food : MonoBehaviour
{
    public enum FoodType
    {
        Grow,
        Reduce
    }

    public FoodType foodType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Snake snake = collision.GetComponent<Snake>();
        if (snake != null)
        {
            switch (foodType)
            {
                case FoodType.Grow:
                    snake.Grow();
                    break;
                case FoodType.Reduce:
                    snake.Reduce();
                    break;
            }

            Destroy(gameObject);
        }
    }
}

