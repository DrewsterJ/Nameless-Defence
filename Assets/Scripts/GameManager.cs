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
    
    // Enemy spawn points
    public List<GroundTile> spawnPoints;
    
    public float _enemySpawnRate;

    public GameObject enemyPrefab;

    private bool _gameActive;
    
    // Start is called before the first frame update
    void Start()
    {
        gameTiles = new List<GroundTile>(tilemap.GetComponentsInChildren<GroundTile>());
        spawnPoints = gameTiles.FindAll(tile => tile.CompareTag("SpawnPoint"));
        _gameActive = true;

        //StartCoroutine(TurretTargetingCoroutine());
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
        //if (enemy == null)
        //    return;

        enemies.Remove(enemy);
    }

    // Called by the GroundTile script to set the tile that the player is currently hovering over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    public void PerformLeftClick()
    { }

    public void PerformRightClick()
    { }

    // Called by the player to interact with a tile using any hotbar action item
    public void InteractWithTile()
    { }
    
    
    /*IEnumerator TurretTargetingCoroutine()
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
    }*/

    // Spawns enemies at `_enemySpawnRate`
    IEnumerator SpawnEnemyCoroutine()
    {
        while (_gameActive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }
    
    // Spawns an enemy at a random spawn point
    private void SpawnEnemy()
    {
        var randIndex = Random.Range(0, spawnPoints.Count);
        var spawnPoint = spawnPoints[randIndex];

        var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
        AddEnemy(enemy.GetComponent<Enemy>());
    }
}
