using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretPlacementUI : MonoBehaviour
{
    // Public fields
    public GameObject turretBuildMenu;
    
    // Private fields
    private GroundTile turretPlacementPosition; // contains tile where purchased turrets from this build menu will spawn at
    private Turret spawnedTurret; // contains the turret that was built using this UI
    
    // Other labeled fields
    [Space] [Header("Build Menu Button")] 
    public GameObject buildMenuButton;
    public Image buildMenuButtonBackground;
    public Image buildMenuIcon;
    
    [Space]
    [Header("Ranged Turret")]
    public Image rangedTurretIcon;
    public Image rangedTurretGoldIcon;
    public TextMeshProUGUI rangedTurretGoldText;
    public int rangedTurretGoldCost;
    
    [Space]
    [Header("Melee Turret")]
    public Image meleeTurretIcon;
    public Image meleeTurretGoldIcon;
    public TextMeshProUGUI meleeTurretGoldText;
    public int meleeTurretGoldCost;

    void Start()
    {
        SetRangedTurretCost(rangedTurretGoldCost);
        SetMeleeTurretCost(meleeTurretGoldCost);
        
        SetDefaultVisibility();

        turretPlacementPosition = GetComponentInParent<GroundTile>();
        Debug.Assert(turretPlacementPosition != null);
    }

    // Sets the default visibility for all icons/buttons
    public void SetDefaultVisibility()
    {
        turretBuildMenu.SetActive(false);
        buildMenuButton.SetActive(true);
        buildMenuIcon.color = AdjustColorOpacity(buildMenuIcon.color, 0.3f);
    }

    // Sets the ranged turret button's color to reflect the player hovering over the button
    public void SetRangedTurretButtonHover(bool hover)
    {
        if (!GameManager.instance.GameActive) return;
        var opacity = (hover) ? 1.0f : 0.3f;

        rangedTurretIcon.color = AdjustColorOpacity(rangedTurretIcon.color, opacity);
        rangedTurretGoldIcon.color = AdjustColorOpacity(rangedTurretGoldIcon.color, opacity);
        rangedTurretGoldText.color = AdjustColorOpacity(rangedTurretGoldText.color, opacity);
    }
    
    // Sets the ranged turret button's color to reflect the player pressing mouse down or 
    public void SetRangedTurretMousePosition(bool mouseDown)
    {
        if (!GameManager.instance.GameActive) return;
        var opacity = (mouseDown) ? 0.8f : 0.3f;

        rangedTurretIcon.color = AdjustColorOpacity(rangedTurretIcon.color, opacity);
        rangedTurretGoldIcon.color = AdjustColorOpacity(rangedTurretGoldIcon.color, opacity);
        rangedTurretGoldText.color = AdjustColorOpacity(rangedTurretGoldText.color, opacity);
    }
    
    // Sets the melee turret button's color to reflect the player hovering over the button
    public void SetMeleeTurretButtonHover(bool hover)
    {
        if (!GameManager.instance.GameActive) return;
        var opacity = (hover) ? 1.0f : 0.3f;
        
        meleeTurretIcon.color = AdjustColorOpacity(meleeTurretIcon.color, opacity);
        meleeTurretGoldIcon.color = AdjustColorOpacity(meleeTurretGoldIcon.color, opacity);
        meleeTurretGoldText.color = AdjustColorOpacity(meleeTurretGoldText.color, opacity);
    }
    
    // Sets the ranged turret button's color to reflect the player pressing mouse down or up
    public void SetMeleeTurretMousePosition(bool mouseDown)
    {
        if (!GameManager.instance.GameActive) return;
        var opacity = (mouseDown) ? 0.8f : 0.3f;
        
        meleeTurretIcon.color = AdjustColorOpacity(meleeTurretIcon.color, opacity);
        meleeTurretGoldIcon.color = AdjustColorOpacity(meleeTurretGoldIcon.color, opacity);
        meleeTurretGoldText.color = AdjustColorOpacity(meleeTurretGoldText.color, opacity);
    }

    // Sets the build menu button's color to reflect the player hovering over the button
    public void SetBaseButtonHover(bool hover)
    {
        if (!GameManager.instance.GameActive) return;
        var opacity = (hover) ? 1.0f : 0.3f;
        buildMenuIcon.color = AdjustColorOpacity(buildMenuIcon.color, opacity);
    }

    // Sets the internal gold cost of the ranged turret and updates the UI accordingly
    public void SetRangedTurretCost(int value)
    {
        rangedTurretGoldCost = value;
        rangedTurretGoldText.text = value.ToString();
    }

    // Sets the internal gold cost of the melee turret and updates the UI accordingly
    public void SetMeleeTurretCost(int value)
    {
        meleeTurretGoldCost = value;
        meleeTurretGoldText.text = value.ToString();
    }
    
    // Sets the given image to be invisible
    public void SetHidden(GameObject ui)
    {
        ui.SetActive(false);
    }

    // Sets the visibility of the build menu containing purchasable turrets
    public void SetTurretBuildMenuVisibility(bool visible)
    {
        if (!GameManager.instance.GameActive) return;
        turretBuildMenu.SetActive(visible);
    }
    
    // Returns the given color with the given opacity applied to it
    private Color AdjustColorOpacity(Color color, float opacity)
    {
        return new Color(color.r, color.g, color.b, opacity);
    }
    
    // Spawns a ranged turret in the game world using the 
    public void BuildRangedTurret()
    {
        if (!GameManager.instance.GameActive) return;
        if (GameManager.instance.GetPlayerGold() < rangedTurretGoldCost)
            return;
        
        GameManager.instance.ModifyPlayerGold(-rangedTurretGoldCost);
        buildMenuButton.SetActive(false);
        turretBuildMenu.SetActive(false);
        spawnedTurret = TurretManager.instance.SpawnTurretAtTile(turretPlacementPosition);
        Debug.Assert(spawnedTurret != null);
    }

    // Spawns a melee turret in the game world using the TurretManager
    public void BuildMeleeTurret()
    {
        if (!GameManager.instance.GameActive) return;
        if (GameManager.instance.GetPlayerGold() < rangedTurretGoldCost)
            return;
        
        GameManager.instance.ModifyPlayerGold(-meleeTurretGoldCost);
        buildMenuButton.SetActive(false);
        turretBuildMenu.SetActive(false);
    }
}
