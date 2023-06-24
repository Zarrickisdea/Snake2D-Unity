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
        Vector3 offset = new Vector3(16.0f, 0.0f, 0.0f);
        Vector3 nextPartPosition = segments[segments.Count - 1].position + offset;
        Transform snakePart = Instantiate(snakeSegment, nextPartPosition, Quaternion.identity);
        segments.Add(snakePart);
    }
}
