using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] edge;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private Tile[] edgeTiles;
    [SerializeField] private Tile levelTile;
    [SerializeField] private GameObject[] snakeHead;
    [SerializeField] private Transform[] food;
    [SerializeField] private float spawnInterval;
    private Tilemap horizontalEdgeGrid;
    private Tilemap verticalEdgeGrid;
    private Tilemap levelGrid;
    private BoundsInt levelArea;
    private BoundsInt horizontalEdgeArea;
    private BoundsInt verticalEdgeArea;
    private int index;
    private Transform currentFood;
    private List<Vector3> availableEdgePositions = new List<Vector3>();
    private List<Vector3Int> availableLevelPositions = new List<Vector3Int>();
    private List<Vector3> availableSpawnPositions = new List<Vector3>();

    private void Awake()
    {
        horizontalEdgeGrid = Instantiate(edge[0]).GetComponentInChildren<Tilemap>();
        verticalEdgeGrid = Instantiate(edge[1]).GetComponentInChildren<Tilemap>();

        index = Random.Range(0, levels.Length);
        levelGrid = Instantiate(levels[index]).GetComponentInChildren<Tilemap>();
        horizontalEdgeArea = horizontalEdgeGrid.cellBounds;
        verticalEdgeArea = verticalEdgeGrid.cellBounds;
        levelArea = levelGrid.cellBounds;
        Spawn();
        GameBounds.InitializeBounds(availableEdgePositions);
    }

    private void Start()
    {
        SpawnTiles();
        SpawnSnake();
        StartCoroutine(SpawnFood());
    }

    private IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (food.Length == 0 || availableSpawnPositions.Count == 0)
                continue;

            if (currentFood != null)
            {
                Destroy(currentFood.gameObject);
            }

            Transform randomFoodPrefab = food[Random.Range(0, food.Length)];
            Vector3 randomSpawnPosition = availableSpawnPositions[Random.Range(0, availableSpawnPositions.Count)];

            currentFood = Instantiate(randomFoodPrefab, randomSpawnPosition, Quaternion.identity);
        }
    }

    private void Spawn()
    {
        foreach (Vector3Int position in horizontalEdgeArea.allPositionsWithin)
        {
            if (IsEdgeCellY(position))
            {
                horizontalEdgeGrid.SetTile(position, edgeTiles[0]);
                Vector3 edgeTemp = horizontalEdgeGrid.GetCellCenterWorld(position);
                availableEdgePositions.Add(edgeTemp);
            }
        }

        foreach (Vector3Int position in verticalEdgeArea.allPositionsWithin)
        {
            if (IsEdgeCellX(position))
            {
                verticalEdgeGrid.SetTile(position, edgeTiles[1]);
                Vector3 edgeTemp = verticalEdgeGrid.GetCellCenterWorld(position);
                availableEdgePositions.Add(edgeTemp);
            }
        }

        foreach (Vector3Int position in levelArea.allPositionsWithin)
        {
            if (!horizontalEdgeGrid.HasTile(position) && !verticalEdgeGrid.HasTile(position))
            {
                availableLevelPositions.Add(position);
            }
        }
    }

    private void SpawnSnake()
    {
        Vector3 spawn1 = GetRandomSpawnPosition();
        Vector3 spawn2 = GetRandomSpawnPosition();

        PlayerInput player1 = PlayerInput.Instantiate(snakeHead[0], controlScheme: "Keyboard");
        PlayerInput player2 = PlayerInput.Instantiate(snakeHead[1], controlScheme: "Keyboard2");

        InputUser.PerformPairingWithDevice(Keyboard.current, player2.user);
        player2.user.ActivateControlScheme("Keyboard2");

        player1.transform.position = spawn1;
        player2.transform.position = spawn2;
    }

    private void SpawnTiles()
    {
        foreach (Vector3Int position in availableLevelPositions)
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
                if (!availableLevelPositions.Contains(adjacentPosition))
                {
                    continue;
                }

                levelGrid.SetTile(adjacentPosition, levelTile);
            }
        }
    }

    public Vector3 GetRandomSpawnPosition()
    {
        int randomIndex = Random.Range(0, availableSpawnPositions.Count);
        return availableSpawnPositions[randomIndex];
    }

    private bool IsEdgeCellX(Vector3Int position)
    {
        return position.x == horizontalEdgeArea.xMin || position.x == horizontalEdgeArea.xMax - 1;
    }

    private bool IsEdgeCellY(Vector3Int position)
    {
        return position.y == verticalEdgeArea.yMin || position.y == verticalEdgeArea.yMax - 1;
    }
}
