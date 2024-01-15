using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretPlacementUI : MonoBehaviour
{
    public Image buildMenuButton;
    public Image buildMenuIcon;
    public Image turretOption1;
    public Image turretOption1Icon;
    public Image turretOption1GoldIcon;
    public TextMeshProUGUI turretOption1GoldCostText;
    
    public Image turretOption2;
    public Image turretOption2Icon;
    public Image turretOption2GoldIcon;
    public TextMeshProUGUI turretOption2GoldCostText;

    public int turretOption1GoldCost;
    public int turretOption2GoldCost;

    public GroundTile turretPlacementPosition;
    
    // Turret 1
    private Color prevTurretOneGoldIconColor;
    private Color prevTurretOneGoldTextColor;
    private Color prevTurretOneIconColor;
    
    // Turret 2
    private Color prevTurretTwoGoldIconColor;
    private Color prevTurretTwoGoldTextColor;
    private Color prevTurretTwoIconColor;

    void Start()
    {
        SetTurretOption1GoldCost(100);
        SetTurretOption2GoldCost(250);
        SetDefaultVisiblity();

        turretPlacementPosition = GetComponentInParent<GroundTile>();
        
        Debug.Assert(turretPlacementPosition != null);
    }

    // Sets UI element visibility to the default that is set at the beginning of the game
    public void SetDefaultVisiblity()
    {
        SetHidden(turretOption1);
        SetHidden(turretOption2);
        SetHidden(turretOption1Icon);
        SetHidden(turretOption2Icon);
        SetHidden(turretOption1GoldIcon);
        SetHidden(turretOption2GoldIcon);
        SetHidden(turretOption1GoldCostText);
        SetHidden(turretOption2GoldCostText);
        SetBaseButtonDefault(buildMenuButton);
        SetBaseButtonDefault(buildMenuIcon);
    }

    // Sets the gold cost for turret option 1 (ranged turret) and updates UI text accordingly
    void SetTurretOption1GoldCost(int amount)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        Debug.Assert(amount > 0);
        turretOption1GoldCost = amount;
        turretOption1GoldCostText.text = amount.ToString();
    }
    
    // Sets the gold cost for turret option 2 (melee turret) and updates UI text accordingly
    void SetTurretOption2GoldCost(int amount)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        Debug.Assert(amount > 0);
        turretOption2GoldCost = amount;
        turretOption2GoldCostText.text = amount.ToString();
    }
    
    // Sets the given image to be invisible
    public void SetHidden(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        image.enabled = false;
    }

    // Sets the given TMP text to be invisible
    public void SetHidden(TextMeshProUGUI text)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
        text.enabled = false;
    }

    public void SetBaseButtonDefault(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
        image.enabled = true;
    }

    // Sets the given image to be a default visibility
    public void SetDefault(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    
    // Sets the given TMP text to be a default visibility
    public void SetDefault(TextMeshProUGUI text)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.3f);
        //text.enabled = true;
    }

    // Sets the given image to be enabled
    public void SetEnabled(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.enabled = true;
    }
    
    // Sets the given TMP text to be enabled
    public void SetEnabled(TextMeshProUGUI text)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        text.enabled = true;
    }

    // Sets the given image to reflect being hovered on
    public void SetHovered(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }
    
    // Sets the given TMP text to reflect being hovered on
    public void SetHovered(TextMeshProUGUI text)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }
    
    // Sets the given TMP text to reflect being clicked
    public void SetMouseDown(TextMeshProUGUI text)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.8f);
    }
    
    // Sets the given image reflect being clicked
    public void SetMouseDown(Image image)
    {
        if (!GameManager.instance._gameActive)
            return;
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.8f);
    }

    public void SetMouseDownOnTurretOne()
    { 
        if (!GameManager.instance._gameActive)
            return;
        
        prevTurretOneGoldIconColor = turretOption1GoldIcon.color; 
        prevTurretOneGoldTextColor = turretOption1GoldCostText.color;
        prevTurretOneIconColor = turretOption1Icon.color;
        
        SetMouseDown(turretOption1GoldIcon);
        SetMouseDown(turretOption1GoldCostText);
        SetMouseDown(turretOption1Icon);
    }

    public void SetMouseUpOnTurretOne()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        turretOption1GoldIcon.color = prevTurretOneGoldIconColor; 
        turretOption1GoldCostText.color = prevTurretOneGoldTextColor;
        turretOption1Icon.color = prevTurretOneIconColor;
    }
    
    public void SetMouseDownOnTurretTwo()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        prevTurretTwoGoldIconColor = turretOption2GoldIcon.color; 
        prevTurretTwoGoldTextColor = turretOption2GoldCostText.color;
        prevTurretTwoIconColor = turretOption2Icon.color;
        
        SetMouseDown(turretOption2GoldIcon);
        SetMouseDown(turretOption2GoldCostText);
        SetMouseDown(turretOption2Icon);
    }

    public void SetMouseUpOnTurretTwo()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        turretOption2GoldIcon.color = prevTurretTwoGoldIconColor; 
        turretOption2GoldCostText.color = prevTurretTwoGoldTextColor;
        turretOption2Icon.color = prevTurretTwoIconColor;
    }

    public void HideAllUI()
    {
        SetHidden(turretOption1);
        SetHidden(turretOption2);
        SetHidden(turretOption1Icon);
        SetHidden(turretOption2Icon);
        SetHidden(turretOption1GoldIcon);
        SetHidden(turretOption2GoldIcon);
        SetHidden(turretOption1GoldCostText);
        SetHidden(turretOption2GoldCostText);
        SetHidden(buildMenuButton);
        SetHidden(buildMenuIcon);
    }

    public void BuildRangedTurret()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        GameManager.instance.SpawnTurretAtTile(turretPlacementPosition);
        HideAllUI();
    }

    public void BuildMeleeTurret()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        // Do nothing for now
    }
}
