using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private int snakeSize;
    [SerializeField] Transform snakeSegment;
    private List<Transform> segments = new List<Transform>();

    private void Awake()
    {
        SetSnake();
    }

    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        float x = Mathf.Round(transform.position.x) + 2.0f;
        float y = Mathf.Round(transform.position.y) + 0;

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
