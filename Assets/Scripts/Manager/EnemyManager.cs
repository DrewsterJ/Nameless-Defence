using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public static EnemyManager instance;
    public GameObject enemyPrefab; 
    public float spawnRate; // how often enemies spawn
    public List<GroundTile> spawnPoints; // where enemies can spawn
    private Coroutine _enemySpawnCoroutine;
    
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
    }
    
    // Event is invoked by the GameManager when the game starts or restarts
    void OnGameStart()
    {
        RemoveAllEnemies();
        _enemySpawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }

    // Event is invoked by the GameManager when the player's base's health reaches 0
    void OnGameOver()
    {
        StopCoroutine(_enemySpawnCoroutine);
        foreach (var enemy in enemies)
            enemy.movementSpeed = 0;
    }
    
    // Controls enemy spawning
    public IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemy();
        }
    }
    
    // Spawns an enemy at a random spawn point
    [RequiresGameActive]
    private void SpawnEnemy()
    {
        var randIndex = Random.Range(0, spawnPoints.Count);
        var spawnPoint = spawnPoints[randIndex];

        var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
        enemies.Add(enemy.GetComponent<Enemy>());
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
