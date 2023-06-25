using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [SerializeField] private int snakeSize;
    [SerializeField] Transform snakeSegment;
    [SerializeField] private float moveSpeed;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 lastDirection;
    private List<Transform> segments = new List<Transform>();

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        SetSnake();
    }

    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
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
        lastDirection = currentDirection;
        Vector2 movement = currentDirection * moveSpeed;
        for (int i = segments.Count - 1; i > 0; i--)
        {
            Vector2 newPosition = (Vector2)segments[i - 1].position - movement.normalized * 16.0f;
            segments[i].position = newPosition;
        }

        float x = Mathf.Round(transform.position.x) + movement.x;
        float y = Mathf.Round(transform.position.y) + movement.y;

        if (x < GameBounds.Left)
            x = GameBounds.Right;
        else if (x > GameBounds.Right)
            x = GameBounds.Left;

        if (y < GameBounds.Bottom)
            y = GameBounds.Top;
        else if (y > GameBounds.Top)
            y = GameBounds.Bottom;

        transform.position = new Vector2(x, y);

        if (currentDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (currentDirection == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (currentDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (currentDirection == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
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
