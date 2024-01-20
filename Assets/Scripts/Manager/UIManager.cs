using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject gameOverCanvas;
    public FloatingHealthBar _baseHealthBar;
    public Image gameOverRestartButton;
    public Image gameOverExitButton;
    public TextMeshProUGUI playerGoldText;
    public TextMeshProUGUI gameWaveNumberText;

    void Start()
    {
        Debug.Assert(_baseHealthBar != null && !_baseHealthBar.IsUnityNull());
    }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    void OnGameStart()
    {
        _baseHealthBar.SetMaxHealth(GameManager.instance._maxBaseHealth);
        _baseHealthBar.SetHealth(GameManager.instance._maxBaseHealth);
        SetGameOverUIVisibility(false);
        ResetTurretPlacementOverlays();
    }

    void OnGameOver()
    {
        SetGameOverUIVisibility(true);
    }

    void OnEnable()
    {
        GameManager.instance.onGameStart += OnGameStart;
        GameManager.instance.onGameOver += OnGameOver;
    }

    void OnDisable()
    {
        GameManager.instance.onGameStart -= OnGameStart;
        GameManager.instance.onGameOver -= OnGameOver;
    }
    
    // Sets the UI player gold text to the given amount
    public void SetPlayerGold(int amount)
    {
        amount = (amount < 0) ? 0 : amount;
        playerGoldText.text = amount.ToString();
    }
    
    // Sets the UI wave number text
    public void SetGameWave(int wave)
    {
        if (!GameManager.instance.GameActive) return;
        gameWaveNumberText.text = wave.ToString();
    }

    // Sets the restart button's color to reflect the user's mouse hovering over it
    public void SetRestartButtonHover(bool hover)
    {
        gameOverRestartButton.color = (hover) ?  Color.red : Color.white;
    }

    // Sets the quit button's color to reflect the user's mouse hovering over it
    public void SetQuitButtonHover(bool hover)
    {
        gameOverExitButton.color = (hover) ?  Color.red : Color.white;
    }

    // Sets the visibility of the game over UI
    public void SetGameOverUIVisibility(bool visible)
    {
        gameOverCanvas.SetActive(visible);
    }
    
    // Resets all turret placement UIs to the default state
    public void ResetTurretPlacementOverlays()
    {
        foreach (var tile in GameManager.instance.gameTiles)
        {
            if (tile.GetComponentInChildren<TurretPlacementUI>() != null)
                tile.GetComponentInChildren<TurretPlacementUI>().SetDefaultVisibility();
        }
    }
}
