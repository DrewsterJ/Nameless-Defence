using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public static EnemyManager instance;
    public GameObject enemyPrefab; 
    public float spawnRate; // how often enemies spawn
    public List<GroundTile> spawnPoints; // where enemies can spawn
    private Coroutine _enemySpawnCoroutine;
    public int numEnemiesToSpawn; // number of enemies that should be spawned in the current "wave"
    private int baseEnemySpawnAmount = 10;
    public event UnityAction OnWaveOver;
    
    void Start()
    {
        var gameTiles = GameManager.instance.gameTiles;
        spawnPoints = gameTiles.FindAll(tile => tile.CompareTag("SpawnPoint"));
    }
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        GameManager.instance.onGameOver += OnGameOver;
        GameManager.instance.onGameStart += OnGameStart;
        GameManager.instance.onNewWave += OnNewWave;
    }
    
    // Event is invoked by the GameManager when the game starts or restarts
    void OnGameStart()
    {
        RemoveAllEnemies();
    }

    // Event is invoked by the GameManager when the player's base's health reaches 0
    void OnGameOver()
    {
        foreach (var enemy in enemies)
            enemy.StopMovement();
    }
    
    void OnNewWave()
    {
        var generalEnemySpawnAmount = GameManager.instance.wave + baseEnemySpawnAmount;
        var enemySpawnAmountOffset = Convert.ToInt32(generalEnemySpawnAmount * .25);
        numEnemiesToSpawn = generalEnemySpawnAmount + enemySpawnAmountOffset;

        if (spawnRate >= .10)
            spawnRate -= .025f;
        
        _enemySpawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }
    
    // Controls enemy spawning
    public IEnumerator SpawnEnemyCoroutine()
    {
        while (numEnemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemy();
        }
        
        OnWaveOver.Invoke();
    }
    
    // Spawns an enemy at a random spawn point
    private void SpawnEnemy()
    {
        if (!GameManager.instance.GameActive) return;
        var randIndex = Random.Range(0, spawnPoints.Count);
        var spawnPoint = spawnPoints[randIndex];

        var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
        enemies.Add(enemy.GetComponent<Enemy>());
        --numEnemiesToSpawn;
    }
    
    // Removes turrets that no longer exist or were destroyed
    public void RemoveInvalidEnemies()
    {
        enemies.RemoveAll(enemy =>
        {
            return (enemy == null || enemy.IsDestroyed() || enemy.IsUnityNull());
        });
    }

    // Despawns all enemies from the game world
    public void RemoveAllEnemies()
    {
        foreach (var enemy in enemies.Where(enemy =>
                     !enemy.IsUnityNull() &&
                     !enemy.IsDestroyed()))
        {
            enemy.gameObject.SetActive(false);
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }
}
