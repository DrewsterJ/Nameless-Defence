using System.Collections;
using System.Collections.Generic;
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

    public Image cancelButton;
    public Image turretOption1;
    public Image turretOption2;
    
    private Color turretBuildButtonBackgroundInactiveColor = new Color(173, 173, 173, 0.0f);
    private Color turretBuildButtonBackgroundActiveColor = new Color(173, 173, 173, 0.3f);
    
    private Color turretBuildButtonIconInactiveColor = new Color(255, 255, 255, 0.3f);
    private Color turretBuildButtonIconActiveColor = new Color(255, 255, 255, 0.6f);
    
    private Color turretSelectionButtonInactiveColor = new Color(173, 173, 173, 0.2f);
    private Color turretSelectionButtonActiveColor = new Color(173, 173, 173, 0.3f);
    
    void Start()
    {
        SetBaseButtonAndIconOff();
        
        cancelButton.enabled = false;
        turretOption1.enabled = false;
        turretOption2.enabled = false;
    }

    public void SetTurretSelectionOptionOn(Image image)
    {
        image.enabled = true;
        image.color = turretSelectionButtonInactiveColor;
    }

    public void SetTurretSelectionOptionOff(Image image)
    {
        image.enabled = false;
        image.color = turretSelectionButtonInactiveColor;
    }

    public void SetTurretSelectionOptionHoverOn(Image image)
    {
        image.color = turretSelectionButtonActiveColor;
    }
    
    public void SetTurretSelectionOptionHoverOff(Image image)
    {
        image.color = turretSelectionButtonInactiveColor;
    }

    public void MouseHoverOn()
    {
        SetBaseButtonAndIconOn();
    }

    public void SetBaseButtonAndIconOn()
    {
        buildMenuButton.color = turretBuildButtonBackgroundActiveColor;
        buildMenuIcon.color = turretBuildButtonIconActiveColor;
    }

    public void SetBaseButtonAndIconOff()
    {
        buildMenuButton.color = turretBuildButtonBackgroundInactiveColor;
        buildMenuIcon.color = turretBuildButtonIconInactiveColor;
    }
}
