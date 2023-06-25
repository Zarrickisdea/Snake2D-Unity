using System.Collections.Generic;
using UnityEngine;

public static class GameBounds
{
    private static float left;
    private static float right;
    private static float top;
    private static float bottom;

    public static void InitializeBounds(List<Vector3> availableEdgePositions)
    {
        // Calculate the minimum and maximum x and y values from the available edge positions
        left = float.MaxValue;
        right = float.MinValue;
        top = float.MinValue;
        bottom = float.MaxValue;

        foreach (Vector3 position in availableEdgePositions)
        {
            if (position.x < left)
                left = position.x + 36.0f;
            if (position.x > right)
                right = position.x - 36.0f;
            if (position.y > top)
                top = position.y - 36.0f;
            if (position.y < bottom)
                bottom = position.y + 36.0f;
        }
    }

    public static float Left => left;
    public static float Right => right;
    public static float Top => top;
    public static float Bottom => bottom;
}

