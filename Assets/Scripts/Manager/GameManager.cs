using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject tileMap;
    public List<GroundTile> gameTiles;
    public static bool GameActive => _gameActive;
    public static bool _gameActive;
    private int _baseHealth;
    public int _maxBaseHealth;
    private int _playerGold;
    
    public event UnityAction onGameOver;
    public event UnityAction onGameStart;
    
    void Start()
    {
        gameTiles = new List<GroundTile>(tileMap.GetComponentsInChildren<GroundTile>());
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
    [RequiresGameActive]
    public void ModifyPlayerGold(int amount)
    {
        _playerGold += amount;
        UIManager.instance.SetPlayerGold(_playerGold);
    }

    public int GetPlayerGold()
    {
        return _playerGold;
    }
    
    // Inflicts the given damage against the player's base's health
    public void DamageBase(int damage)
    {
        damage = (damage < 0) ? 0 : damage;
        _baseHealth -= damage;
        UIManager.instance._baseHealthBar.SetHealth(_baseHealth);

        if (_baseHealth <= 0)
            GameOver();
    }
    
    // Heals the player's base for the given amount
    public void HealBase(int health)
    {
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
        
        // Set initial gold amount to 400
        _playerGold = 0;
        ModifyPlayerGold(400);
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequiresGameActiveAttribute : Attribute {}