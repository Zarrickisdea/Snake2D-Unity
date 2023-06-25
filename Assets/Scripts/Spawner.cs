using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject edge;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private Tile edgeTile;
    [SerializeField] private Tile levelTile;
    [SerializeField] private GameObject[] snakeHead;
    [SerializeField] private Transform[] food;
    [SerializeField] private float spawnInterval;
    private Tilemap edgeGrid;
    private Tilemap levelGrid;
    private BoundsInt levelArea;
    private BoundsInt edgeArea;
    private int index;
    private Transform currentFood;
    private List<Vector3> availableEdgePositions = new List<Vector3>();
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
        foreach (Vector3Int position in edgeArea.allPositionsWithin)
        {
            if (IsEdgeCell(position))
            {
                edgeGrid.SetTile(position, edgeTile);
                Vector3 edgeTemp = edgeGrid.GetCellCenterWorld(position);
                availableEdgePositions.Add(edgeTemp);
            }
        }

        foreach (Vector3Int position in levelArea.allPositionsWithin)
        {
            if (!edgeGrid.HasTile(position))
            {
                availableLevelPositions.Add(position);
            }
        }

        SpawnTiles();
        GameBounds.InitializeBounds(availableEdgePositions);
        SpawnSnake();
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

    private bool IsEdgeCell(Vector3Int position)
    {
        return position.x == edgeArea.xMin || position.x == edgeArea.xMax - 1 || position.y == edgeArea.yMin || position.y == edgeArea.yMax - 1;
    }
}
