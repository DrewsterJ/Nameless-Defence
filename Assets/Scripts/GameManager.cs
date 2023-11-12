using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject tilemap;
    private List<GroundTile> gameTiles;

    // The tile the player is hovering their mouse over
    private GroundTile focusedTile;

    public float _turretTargetAcquisitionInterval = 1;
    public float _enemyTargetAcquisitionInterval = 1;

    public List<Enemy> enemies;
    public List<Turret> turrets;

    private bool _gameActive;

    public PlayerController player;

    public float _enemySpawnRate;

    public float _minimumEnemySpawnDistanceFromPlayer;

    public GameObject enemyPrefab;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            _gameActive = true;
            instance = this;
            gameTiles = new List<GroundTile>(tilemap.GetComponentsInChildren<GroundTile>());
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    public void AddTurret(Turret turret)
    {
        if (turret == null)
            return;
        
        turrets.Add(turret);
    }
    
    public void RemoveTurret(Turret turret)
    {
        if (turret == null)
            return;

        turrets.Remove(turret);
    }

    public void AddEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;
        
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        enemies.Remove(enemy);
    }

    // Called by the GroundTile script to set the tile that the player is currently hovering over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    public void PerformPlayerLeftClick(PlayerController player)
    {
        // If the user is attempting to build
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            InteractWithFocusedTile(player);
        }
    }

    public void PerformPlayerRightClick(PlayerController player)
    {
        // User is opening the build menu
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            var action = player.hotbar.GetActiveHotbarAction() as BuildAction;
            action!.SetBuildMenuVisibility(!action.GetBuildMenuVisibility());
        }
    }

    // Called by the player to interact with a tile using any hotbar action item
    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null) 
            return;
        
        var hotbar = player.hotbar;
        var selectedAction = hotbar.GetActiveHotbarAction();

        if (selectedAction.actionType == Action.ActionType.Empty)
        {
            // Do nothing
        }
        else if (selectedAction.actionType == Action.ActionType.Build)
        {
            var action = selectedAction as BuildAction;
            PerformBuildActionOnTile(action, focusedTile);
        }
    }

    private void PerformBuildActionOnTile(BuildAction action, GroundTile tileToBuildOn)
    {
        Debug.Assert(action != null);
        Debug.Assert(tileToBuildOn != null);

        var distance = Vector2.Distance(action.transform.position, tileToBuildOn.transform.position);
        const float actionRange = 2.0f;

        // Don't perform actions if the player is out of range
        if (distance > actionRange)
            return;
        
        var building = action.selectedBuilding;
        Instantiate(building, tileToBuildOn.transform);

        if (building.CompareTag("Turret"))
            AddTurret(building.GetComponent<Turret>());
    }
    
    IEnumerator TurretTargetingCoroutine()
    {
        while (_gameActive)
        {
            foreach (var turret in turrets.Where(turret => !turret.IsEngagingTarget()))
            {
                foreach (var enemy in enemies)
                {
                    turret.TryEngageTarget(enemy.gameObject);
                    if (turret.IsEngagingTarget())
                        break;
                }
            }
            
            yield return new WaitForSeconds(_turretTargetAcquisitionInterval);
        }
    }
    
    private IEnumerator EnemyTargetingCoroutine()
    {
        while (_gameActive)
        {
            // Identify attackable targets on map (walls, turrets, player)
            // Identify unengaged enemies
            // For every unengaged enemy
            // - Find an ideal target (based on distance, type of target, etc.)
            // - Enemy engages that target
            yield return new WaitForSeconds(_enemyTargetAcquisitionInterval);
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (_gameActive)
        {
            Debug.Log("Spawning enemies!");
            yield return new WaitForSeconds(_enemySpawnRate);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var tiles = GetValidEnemySpawnTiles();
        var index = Random.Range(0, tiles.Count);
        var tile = tiles[index];

        var newEnemy = Instantiate(enemyPrefab, tile.transform.position, enemyPrefab.transform.rotation);
        AddEnemy(newEnemy.GetComponent<Enemy>());
    }

    // Returns a list of tiles that are within a certain distance from the player
    private List<GroundTile> GetValidEnemySpawnTiles()
    {
        var validTiles = gameTiles.FindAll(
            delegate(GroundTile tile)
            {
                var tileDistanceFromPlayer = Vector2.Distance(tile.transform.position, player.transform.position);
                return (tileDistanceFromPlayer >= _minimumEnemySpawnDistanceFromPlayer);
            }
        );
        return validTiles;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _gameActive = true;

        StartCoroutine(EnemyTargetingCoroutine());
        StartCoroutine(TurretTargetingCoroutine());
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
