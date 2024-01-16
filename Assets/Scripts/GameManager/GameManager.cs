using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject tilemap;
    private List<GroundTile> gameTiles;

    public float _turretTargetAcquisitionInterval = 1;

    public List<Enemy> enemies;
    public List<Turret> turrets;
    
    // Enemy spawn points
    public List<GroundTile> spawnPoints;
    
    public float _enemySpawnRate;

    public GameObject enemyPrefab;
    public GameObject turretPrefab;

    public bool _gameActive;

    public FloatingHealthBar _baseHealthBar;

    private int _baseHealth;
    public int _maxBaseHealth;

    public event UnityAction onGameOver;
    public event UnityAction onGameRestart;

    public Image gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Image gameOverRestartGameButton;
    public TextMeshProUGUI restartGameText;
    public Image gameOverExitGameButton;
    public TextMeshProUGUI quitGameText;
    
    // Start is called before the first frame update
    void Start()
    {
        gameTiles = new List<GroundTile>(tilemap.GetComponentsInChildren<GroundTile>());
        spawnPoints = gameTiles.FindAll(tile => tile.CompareTag("SpawnPoint"));
        Debug.Assert(_baseHealthBar != null && !_baseHealthBar.IsUnityNull());

        StartGame();
    }

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

    public void SpawnTurretAtTile(GroundTile tile)
    {
        Debug.Assert(tile != null);
        
        var turret = Instantiate(turretPrefab, tile.transform.position, turretPrefab.transform.rotation);
        AddTurret(turret.GetComponent<Turret>());
    }

    IEnumerator TurretTargetingCoroutine()
    {
        while (_gameActive)
        {
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
            RemoveInvalidTurrets();
            yield return new WaitForSeconds(_turretTargetAcquisitionInterval);
        }
    }

    public void RemoveInvalidTurrets()
    {
        var invalidTurrets = turrets.FindAll(turret =>
        {
            return (turret.IsDestroyed() || turret.IsUnityNull());
        });

        foreach (var invalidTurret in invalidTurrets)
        {
            turrets.Remove(invalidTurret);
        }
    }

    // Spawns enemies at `_enemySpawnRate`
    IEnumerator SpawnEnemyCoroutine()
    {
        while (_gameActive)
        {
            yield return new WaitForSeconds(_enemySpawnRate);
            SpawnEnemy();
        }
    }
    
    // Spawns an enemy at a random spawn point
    private void SpawnEnemy()
    {
        var randIndex = Random.Range(0, spawnPoints.Count);
        var spawnPoint = spawnPoints[randIndex];

        var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
        AddEnemy(enemy.GetComponent<Enemy>());
        //DamageBase(25);
    }

    public void DamageBase(int damage)
    {
        if (damage < 0)
            damage = 0;

        _baseHealth -= damage;
        _baseHealthBar.SetHealth(_baseHealth);

        if (_baseHealth <= 0)
            GameOver();
    }

    public void HealBase(int health)
    {
        if (health < 0)
            health = 0;

        _baseHealth += health;
        _baseHealthBar.SetHealth(_baseHealth);
    }

    private void GameOver()
    {
        ShowGameOverUI();
        StopCoroutine(SpawnEnemyCoroutine());
        StopCoroutine(TurretTargetingCoroutine());

        foreach (var turret in turrets)
        {
            turret.DisengageActiveTarget();
        }

        foreach (var enemy in enemies)
        {
            //enemy._movementSpeed = 0;
        }
        
        foreach (var tile in gameTiles)
        {
            if (tile.GetComponentInChildren<TurretPlacementUI>() != null)
                tile.GetComponentInChildren<TurretPlacementUI>().SetDefaultVisiblity();
        }
        
        _gameActive = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        _gameActive = true;
        HideGameOverUI();
        _baseHealth = _maxBaseHealth;

        foreach (var turret in turrets)
        {
            if (!turret.IsUnityNull() && !turret.IsDestroyed())
            {
                turret.gameObject.SetActive(false);
                Destroy(turret.gameObject);
            }
        }
        turrets.Clear();
        
        foreach (var enemy in enemies)
        {
            if (!enemy.IsUnityNull() && !enemy.IsDestroyed())
            {
                enemy.gameObject.SetActive(false);
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
        
        foreach (var tile in gameTiles)
        {
            if (tile.GetComponentInChildren<TurretPlacementUI>() != null)
            {
                tile.GetComponentInChildren<TurretPlacementUI>().SetDefaultVisiblity();
            }
        }
        
        _baseHealthBar.SetMaxHealth(_maxBaseHealth);
        _baseHealthBar.SetHealth(_maxBaseHealth);

        StartCoroutine(TurretTargetingCoroutine());
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void SetRestartButtonHoverOn()
    {
        gameOverRestartGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 0.8f);
        restartGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 0.8f);
    }
    
    public void SetRestartButtonHoverOff()
    {
        gameOverRestartGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 1.0f);
        restartGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 1.0f);
    }
    
    public void SetQuitButtonHoverOn()
    {
        gameOverExitGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 0.8f);
        quitGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 0.8f);
    }
    
    public void SetQuitButtonHoverOff()
    {
        gameOverExitGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 1.0f);
        quitGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 1.0f);
    }

    public void ShowGameOverUI()
    {
        gameOverPanel.color = new Color(gameOverPanel.color.r, gameOverPanel.color.g, gameOverPanel.color.b, 1.0f);
        gameOverPanel.enabled = true;
        
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 1.0f);
        gameOverText.enabled = true;
        
        gameOverRestartGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 1.0f);
        gameOverRestartGameButton.enabled = true;
        restartGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 1.0f);
        restartGameText.enabled = true;
        
        gameOverExitGameButton.color = new Color(gameOverExitGameButton.color.r, gameOverExitGameButton.color.g, gameOverExitGameButton.color.b, 1.0f);
        gameOverExitGameButton.enabled = true;
        quitGameText.color = new Color(quitGameText.color.r, quitGameText.color.g, quitGameText.color.b, 1.0f);
        quitGameText.enabled = true;
    }

    public void HideGameOverUI()
    {
        gameOverPanel.color = new Color(gameOverPanel.color.r, gameOverPanel.color.g, gameOverPanel.color.b, 0.0f);
        gameOverPanel.enabled = false;
        
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0.0f);
        gameOverText.enabled = false;
        
        gameOverRestartGameButton.color = new Color(gameOverRestartGameButton.color.r, gameOverRestartGameButton.color.g, gameOverRestartGameButton.color.b, 0.0f);
        gameOverRestartGameButton.enabled = false;
        restartGameText.color = new Color(restartGameText.color.r, restartGameText.color.g, restartGameText.color.b, 0.0f);
        restartGameText.enabled = false;
        
        gameOverExitGameButton.color = new Color(gameOverExitGameButton.color.r, gameOverExitGameButton.color.g, gameOverExitGameButton.color.b, 0.0f);
        gameOverExitGameButton.enabled = false;
        quitGameText.color = new Color(quitGameText.color.r, quitGameText.color.g, quitGameText.color.b, 0.0f);
        quitGameText.enabled = false;
    }
}
