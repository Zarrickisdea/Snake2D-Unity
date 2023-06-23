using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Tilemap edge;
    [SerializeField] private Tilemap[] levels;
    [SerializeField] private Tile edgeTile;
    [SerializeField] private Tile levelTile;
    private BoundsInt levelArea;
    private BoundsInt edgeArea;
    private int index;

    void Start()
    {
        edgeArea = edge.cellBounds;
        index = Random.Range(0, 2);
        levelArea = levels[index].cellBounds;
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

        SpawnTiles(availableLevelPositions);
    }


    private void SpawnTiles(List<Vector3Int> availablePositions)
    {
        foreach (Vector3Int position in availablePositions)
        {
            if (levels[index].HasTile(position))
            {
                continue;
            }

            bool spawnSingleSpace = Random.value < 0.5f;
            if (spawnSingleSpace)
            {
                continue;
            }

            int randomSize = Random.Range(1, 3);
            for (int i = 0; i < randomSize; i++)
            {
                Vector3Int adjacentPosition = new Vector3Int(position.x + i, position.y, position.z);
                if (!availablePositions.Contains(adjacentPosition))
                {
                    continue;
                }

                levels[index].SetTile(adjacentPosition, levelTile);
            }
        }
    }

    private bool IsEdgeCell(Vector3Int position)
    {
        return position.x == edgeArea.xMin || position.x == edgeArea.xMax - 1 || position.y == edgeArea.yMin || position.y == edgeArea.yMax - 1;
    }
}
