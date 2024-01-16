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

    public GameObject tileMap;
    public List<GroundTile> gameTiles;

    public bool _gameActive;

    public FloatingHealthBar _baseHealthBar;

    private int _baseHealth;
    public int _maxBaseHealth;

    public event UnityAction onGameOver;
    public event UnityAction onGameRestart;

    public GameObject gameOverCanvas;
    
    public Image gameOverRestartGameButton;
    public TextMeshProUGUI restartGameText;
    public Image gameOverExitGameButton;
    public TextMeshProUGUI quitGameText;
    
    // Start is called before the first frame update
    void Start()
    {
        gameTiles = new List<GroundTile>(tileMap.GetComponentsInChildren<GroundTile>());
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
        StopCoroutine(EnemyManager.instance.SpawnEnemyCoroutine());
        StopCoroutine(TurretManager.instance.TurretTargetingCoroutine());

        foreach (var turret in TurretManager.instance.turrets)
        {
            turret.DisengageActiveTarget();
        }

        foreach (var enemy in EnemyManager.instance.enemies)
        {
            enemy.movementSpeed = 0;
        }
        
        foreach (var tile in gameTiles)
        {
            if (tile.GetComponentInChildren<TurretPlacementUI>() != null)
                tile.GetComponentInChildren<TurretPlacementUI>().SetDefaultVisibility();
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

        foreach (var turret in TurretManager.instance.turrets)
        {
            if (!turret.IsUnityNull() && !turret.IsDestroyed())
            {
                turret.gameObject.SetActive(false);
                Destroy(turret.gameObject);
            }
        }
        TurretManager.instance.turrets.Clear();
        
        foreach (var enemy in EnemyManager.instance.enemies)
        {
            if (!enemy.IsUnityNull() && !enemy.IsDestroyed())
            {
                enemy.gameObject.SetActive(false);
                Destroy(enemy.gameObject);
            }
        }
        EnemyManager.instance.enemies.Clear();
        
        foreach (var tile in gameTiles)
        {
            if (tile.GetComponentInChildren<TurretPlacementUI>() != null)
            {
                tile.GetComponentInChildren<TurretPlacementUI>().SetDefaultVisibility();
            }
        }
        
        _baseHealthBar.SetMaxHealth(_maxBaseHealth);
        _baseHealthBar.SetHealth(_maxBaseHealth);

        StartCoroutine(TurretManager.instance.TurretTargetingCoroutine());
        StartCoroutine(EnemyManager.instance.SpawnEnemyCoroutine());
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
        gameOverCanvas.SetActive(true);
    }

    public void HideGameOverUI()
    {
        gameOverCanvas.SetActive(false);
    }
}
