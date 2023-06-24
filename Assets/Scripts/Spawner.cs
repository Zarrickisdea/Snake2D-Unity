using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject edge;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private Tile edgeTile;
    [SerializeField] private Tile levelTile;
    [SerializeField] private GameObject snake;
    private Tilemap edgeGrid;
    private Tilemap levelGrid;
    private BoundsInt levelArea;
    private BoundsInt edgeArea;
    private int index;
    private List<Vector3Int> availableEdgePositions = new List<Vector3Int>();
    private List<Vector3Int> availableLevelPositions = new List<Vector3Int>();
    private List<Vector3> availableSpawnPositions = new List<Vector3>();

    private void Awake()
    {
        edgeGrid = Instantiate(edge).GetComponentInChildren<Tilemap>();

        index = Random.Range(0, levels.Length);
        levelGrid = Instantiate(levels[index]).GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        edgeArea = edgeGrid.cellBounds;
        levelArea = levelGrid.cellBounds;
        Spawn();
    }

    private void Spawn()
    {
        foreach (Vector3Int position in edgeArea.allPositionsWithin)
        {
            if (IsEdgeCell(position))
            {
                edgeGrid.SetTile(position, edgeTile);
                availableEdgePositions.Add(position);
            }
        }

        foreach (Vector3Int position in levelArea.allPositionsWithin)
        {
            if (!edgeGrid.HasTile(position))
            {
                availableLevelPositions.Add(position);
            }
        }

        SpawnTiles(availableLevelPositions);
        SpawnSnakeHead(availableSpawnPositions);
    }

    private void SpawnSnakeHead(List<Vector3> availableSpawnPositions)
    {
        int randomIndex = Random.Range(0, availableSpawnPositions.Count);
        Vector3 spawnPosition = availableSpawnPositions[randomIndex];
        Instantiate(snake, spawnPosition, Quaternion.identity);
        Debug.Log(availableSpawnPositions[randomIndex]);
    }

    private void SpawnTiles(List<Vector3Int> availablePositions)
    {
        foreach (Vector3Int position in availablePositions)
        {
            if (levelGrid.HasTile(position))
            {
                continue;
            }

            bool spawnSingleSpace = Random.value < 0.5f;
            if (spawnSingleSpace)
            {
                Vector3 spawnTemp = levelGrid.GetCellCenterWorld(position);
                availableSpawnPositions.Add(spawnTemp);
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

                levelGrid.SetTile(adjacentPosition, levelTile);
            }
        }
    }

    private bool IsEdgeCell(Vector3Int position)
    {
        return position.x == edgeArea.xMin || position.x == edgeArea.xMax - 1 || position.y == edgeArea.yMin || position.y == edgeArea.yMax - 1;
    }
}
