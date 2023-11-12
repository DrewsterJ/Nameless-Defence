using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    
    // Start is called before the first frame update
    void Start()
    {
        _gameActive = true;
        gameTiles = new List<GroundTile>(tilemap.GetComponentsInChildren<GroundTile>());

        StartCoroutine(EnemyTargetingCoroutine());
        StartCoroutine(TurretTargetingCoroutine());
        StartCoroutine(SpawnEnemyCoroutine());
    }
    
    // Update is called once per frame
    void Update()
    { }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
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
        var buildingGameObj = Instantiate(building, tileToBuildOn.transform);

        if (buildingGameObj.CompareTag("Turret"))
            AddTurret(buildingGameObj.GetComponent<Turret>());
    }

    void RemoveInvalidTurrets()
    {
        var invalidTurrets = turrets.FindAll(turret => turret.IsUnityNull() || turret.IsDestroyed());
        foreach (var invalidTurret in invalidTurrets)
            turrets.Remove(invalidTurret);
    }

    void RemoveInvalidEnemies()
    {
        var invalidEnemies = enemies.FindAll(enemy => enemy.IsUnityNull() || enemy.IsDestroyed());
        foreach (var invalidEnemy in invalidEnemies)
            enemies.Remove(invalidEnemy);
    }
    
    IEnumerator TurretTargetingCoroutine()
    {
        while (_gameActive)
        {
            RemoveInvalidTurrets();
            foreach (var turret in turrets.Where(turret => !turret.IsEngagingTarget()))
            {
                if (turret.IsDestroyed() || turret.IsUnityNull())
                    continue;
                
                foreach (var enemy in enemies)
                {
                    if (enemy.IsDestroyed() || enemy.IsUnityNull())
                        continue;

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
            RemoveInvalidEnemies();
            foreach (var enemy in enemies.Where(enemy => !enemy.IsEngagingTarget()))
            {
                if (enemy.IsDestroyed() || enemy.IsUnityNull())
                    continue;

                float closestTurretDistance = 0.0f;
                Turret closestTurret = null;

                if (turrets.Count > 0)
                {
                    if (!turrets[0].IsDestroyed() && !turrets[0].IsUnityNull())
                    {
                        closestTurretDistance = Vector2.Distance(enemy.gameObject.transform.position, turrets[0].gameObject.transform.position);
                        closestTurret = turrets[0];
                    }
                }
                
                foreach (var turret in turrets)
                {
                    if (turret.IsDestroyed() || turret.IsUnityNull())
                        continue;
                    
                    var distance = Vector2.Distance(enemy.gameObject.transform.position, turret.gameObject.transform.position);
                    if (distance < closestTurretDistance)
                    {
                        closestTurretDistance = distance;
                        closestTurret = turret;
                    }
                }

                float distanceFromPlayer = Vector2.Distance(enemy.gameObject.transform.position, player.gameObject.transform.position);
                
                Debug.Log("Distance from player: " + distanceFromPlayer);
                Debug.Log("Distance from turret: " + closestTurretDistance);

                if (closestTurretDistance != 0.0f && distanceFromPlayer != 0.0f)
                    enemy.activeTarget = (closestTurretDistance < distanceFromPlayer) ? closestTurret!.gameObject : player.gameObject;
                else if (closestTurretDistance != 0.0f)
                    enemy.activeTarget = closestTurret.gameObject;
                else
                    enemy.activeTarget = player.gameObject;
            }
            
            yield return new WaitForSeconds(_enemyTargetAcquisitionInterval);
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (_gameActive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_enemySpawnRate);
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
}
