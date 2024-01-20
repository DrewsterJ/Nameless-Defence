using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject tileMap;
    public List<GroundTile> gameTiles;
    public bool GameActive => _gameActive;
    public bool _gameActive;
    private int _baseHealth;
    public int _maxBaseHealth;
    private int _playerGold;
    public int wave;
    private Coroutine _delayWaveStartCoroutine;
    
    public event UnityAction onGameOver;
    public event UnityAction onGameStart;
    public event UnityAction onNewWave;
    
    void Start()
    {
        gameTiles = new List<GroundTile>(tileMap.GetComponentsInChildren<GroundTile>());
        EnemyManager.instance.OnWaveOver += OnWaveOver;
        StartGame();
    }
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Modifies the internal player gold amount and updates the UI with the modified amount
    public void ModifyPlayerGold(int amount)
    {
        if (!GameActive) return;
        _playerGold += amount;
        UIManager.instance.SetPlayerGold(_playerGold);
    }
    
    // Increments the internal wave number and updates the UI with the new wave
    public void IncrementGameWave()
    {
        if (!GameActive) return;
        wave += 1;
        UIManager.instance.SetGameWave(wave);
        onNewWave.Invoke();
    }

    public void OnWaveOver()
    {
        _delayWaveStartCoroutine = StartCoroutine(DelayWaveStartCoroutine());
    }
    
    IEnumerator DelayWaveStartCoroutine()
    {
        if (!GameManager.instance.GameActive) yield return null;
        yield return new WaitForSeconds(10);
        IncrementGameWave();
    }

    private void OnEnable()
    {
        //EnemyManager.instance.OnWaveOver += OnWaveOver;
    }

    private void OnDisable()
    {
        EnemyManager.instance.OnWaveOver -= OnWaveOver;
    }

    public int GetPlayerGold()
    {
        return _playerGold;
    }
    
    // Inflicts the given damage against the player's base's health
    public void DamageBase(int damage)
    {
        if (!GameManager.instance.GameActive) return;
        damage = (damage < 0) ? 0 : damage;
        _baseHealth -= damage;
        UIManager.instance._baseHealthBar.SetHealth(_baseHealth);

        if (_baseHealth <= 0)
            GameOver();
    }
    
    // Heals the player's base for the given amount
    public void HealBase(int health)
    {
        if (!GameManager.instance.GameActive) return;
        health = (health < 0) ? 0 : health;
        _baseHealth += health;
        UIManager.instance._baseHealthBar.SetHealth(_baseHealth);
    }
    
    // Called when base health reaches 0
    private void GameOver()
    {
        onGameOver?.Invoke();
        _gameActive = false;
    }
    
    // Exits the application
    public void Quit()
    {
        Application.Quit();
    }
    
    // Called upon game start or restart
    public void StartGame()
    {
        onGameStart?.Invoke();
        
        // Set game variables
        _baseHealth = _maxBaseHealth;
        _gameActive = true;
        wave = 0;
        IncrementGameWave();
        
        // Set initial gold amount to 400
        _playerGold = 0;
        ModifyPlayerGold(1000);
    }
}