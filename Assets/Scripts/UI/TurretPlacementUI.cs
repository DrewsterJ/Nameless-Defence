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

    void Start()
    {
        SetHidden(turretOption1);
        SetHidden(turretOption2);
        SetHidden(turretOption1Icon);
        SetHidden(turretOption2Icon);
        SetHidden(turretOption1GoldIcon);
        SetHidden(turretOption2GoldIcon);
        
        SetTurretOption1GoldCost(100);
        SetTurretOption2GoldCost(250);
        
        SetHidden(turretOption1GoldCostText);
        SetHidden(turretOption2GoldCostText);
        
        SetDefault(buildMenuButton);
        SetDefault(buildMenuIcon);
    }

    // Sets the gold cost for turret option 1 (ranged turret) and updates UI text accordingly
    void SetTurretOption1GoldCost(int amount)
    {
        Debug.Assert(amount > 0);
        turretOption1GoldCost = amount;
        turretOption1GoldCostText.text = amount.ToString();
    }
    
    // Sets the gold cost for turret option 2 (melee turret) and updates UI text accordingly
    void SetTurretOption2GoldCost(int amount)
    {
        Debug.Assert(amount > 0);
        turretOption2GoldCost = amount;
        turretOption2GoldCostText.text = amount.ToString();
    }
    
    // Sets the given image to be invisible
    public void SetHidden(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        image.enabled = false;
    }

    // Sets the given TMP text to be invisible
    public void SetHidden(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
        text.enabled = false;
    }

    // Sets the given image to be a default visibility
    public void SetDefault(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    
    // Sets the given TMP text to be a default visibility
    public void SetDefault(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.3f);
    }

    // Sets the given image to be enabled
    public void SetEnabled(Image image)
    {
        image.enabled = true;
    }
    
    // Sets the given TMP text to be enabled
    public void SetEnabled(TextMeshProUGUI text)
    {
        text.enabled = true;
    }

    // Sets the given image to reflect being hovered on
    public void SetHovered(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }
    
    // Sets the given TMP text to reflect being hovered on
    public void SetHovered(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }
}
