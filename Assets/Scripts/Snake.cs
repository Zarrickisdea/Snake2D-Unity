using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [SerializeField] private int snakeSize;
    [SerializeField] Transform snakeSegment;
    private Vector2 currentDirection = Vector2.right;
    [SerializeField] private float moveSpeed;
    private List<Transform> segments = new List<Transform>();

    private void Awake()
    {
        SetSnake();
    }

    public void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            Vector2 moveVector = context.ReadValue<Vector2>();
            if (moveVector.magnitude > 0.1f)
            {
                if (Mathf.Approximately(moveVector.x, -currentDirection.x) || Mathf.Approximately(moveVector.y, -currentDirection.y))
                {
                    // Invalid move, ignore it
                    return;
                }
                currentDirection = moveVector;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = currentDirection * moveSpeed;
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        float x = Mathf.Round(transform.position.x) + movement.x;
        float y = Mathf.Round(transform.position.y) + movement.y;

        transform.position = new Vector2(x, y);
    }

    private void SetSnake()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < snakeSize - 1; i++)
        {
            Grow();
        }
    }

    private void Grow()
    {
        Transform snakePart = Instantiate(snakeSegment);
        snakePart.position = segments[segments.Count - 1].position;
        segments.Add(snakePart);
    }
}
