using System.Collections.Generic;
using UnityEngine;

public static class GameBounds
{
    private static float left;
    private static float right;
    private static float top;
    private static float bottom;
    private static float horizontalEdgeTileSize = 36.0f;
    //private static float verticalEdgeTileSize = 40.0f;

    public static void InitializeBounds(List<Vector3> availableEdgePositions)
    {
        // Calculate the minimum and maximum x and y values from the available edge positions
        left = float.MaxValue;
        right = float.MinValue;
        top = float.MinValue;
        bottom = float.MaxValue;

        foreach (Vector3 position in availableEdgePositions)
        {
            // the edgeTileSize is the approximate distance from center of the edgeTile after scaling
            if (position.x < left)
                left = position.x;
            if (position.x > right)
                right = position.x;
            if (position.y > top)
                top = position.y - horizontalEdgeTileSize;
            if (position.y < bottom)
                bottom = position.y + horizontalEdgeTileSize;
        }
    }

    public static float Left => left;
    public static float Right => right;
    public static float Top => top;
    public static float Bottom => bottom;
}

