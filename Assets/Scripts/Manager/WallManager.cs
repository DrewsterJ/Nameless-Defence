using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public static WallManager instance;
    public List<Wall> walls;
    public GameObject wallPrefab;

    public List<GroundTile> wallSpawnPositions;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        Debug.Assert(wallPrefab != null);

        var gameTiles = GameManager.instance.gameTiles;

        // Walls can be spawned at tiles with the WallSpawnPosition tag as well as at all turret placement positions
        wallSpawnPositions = gameTiles.FindAll(tile =>
            tile.CompareTag("WallSpawnPosition") || tile.CompareTag("TurretPlacementPosition"));

        Debug.Assert(wallSpawnPositions.Count == 16);

        foreach (var spawn in wallSpawnPositions)
            SpawnWallAtTile(spawn);
    }

    // Spawns a turret in the world at the given ground tile
    public void SpawnWallAtTile(GroundTile tile)
    {
        if (!GameManager.instance.GameActive) return;
        Debug.Assert(tile != null && !tile.IsDestroyed() && !tile.IsUnityNull());
        var wall = Instantiate(wallPrefab, tile.transform.position, wallPrefab.transform.rotation);
        walls.Add(wall.GetComponent<Wall>());
    }
}