using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public static EnemyManager instance;
    public GameObject enemyPrefab; 
    public float spawnRate; // how often enemies spawn
    public List<GroundTile> spawnPoints; // where enemies can spawn
    
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
}
