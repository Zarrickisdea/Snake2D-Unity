using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Tilemap edge;
    [SerializeField] private Tilemap level;
    [SerializeField] private Tile edgeTile;
    [SerializeField] private Tile levelTile;
    private BoundsInt levelArea;
    private BoundsInt edgeArea;

    void Start()
    {
        edgeArea = edge.cellBounds;
        levelArea = level.cellBounds;
        Spawn();
    }

    private void Spawn()
    {
        List<Vector3Int> availableEdgePositions = new List<Vector3Int>();
        List<Vector3Int> availableLevelPositions = new List<Vector3Int>();

        foreach (Vector3Int position in edgeArea.allPositionsWithin)
        {
            if (IsEdgeCell(position))
            {
                edge.SetTile(position, edgeTile);
                availableEdgePositions.Add(position);
            }
        }

        foreach (Vector3Int position in levelArea.allPositionsWithin)
        {
            if (!edge.HasTile(position))
            {
                availableLevelPositions.Add(position);
            }
        }

        SpawnHorizontalTiles(availableLevelPositions);
    }


    private void SpawnHorizontalTiles(List<Vector3Int> availablePositions)
    {
        foreach (Vector3Int position in availablePositions)
        {
            if (level.HasTile(position))
            {
                continue;
            }

            bool spawnSingleSpace = Random.value < 0.5f;
            if (spawnSingleSpace)
            {
                continue;
            }

            int randomSize = Random.Range(2, 4);
            for (int i = 0; i < randomSize; i++)
            {
                Vector3Int adjacentPosition = new Vector3Int(position.x, position.y + i, position.z);
                if (!availablePositions.Contains(adjacentPosition))
                {
                    continue;
                }

                level.SetTile(adjacentPosition, levelTile);
            }
        }
    }

    private bool IsEdgeCell(Vector3Int position)
    {
        return position.x == edgeArea.xMin || position.x == edgeArea.xMax - 1 || position.y == edgeArea.yMin || position.y == edgeArea.yMax - 1;
    }
}
