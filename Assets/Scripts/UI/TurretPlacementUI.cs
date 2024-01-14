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
    public Image turretOption1;
    public Image turretOption1Icon;
    public Image turretOption2;
    public Image turretOption2Icon;

    void Start()
    {
        SetHidden(turretOption1);
        SetHidden(turretOption2);
        SetHidden(turretOption1Icon);
        SetHidden(turretOption2Icon);
        SetDefault(buildMenuButton);
        SetDefault(buildMenuIcon);
    }

    // Sets the given image to be invisible
    public void SetHidden(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        image.enabled = false;
    }

    // Sets the given image to be a default visibility
    public void SetDefault(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }

    public void SetEnabled(Image image)
    {
        image.enabled = true;
    }

    // Sets the given image to reflect being hovered on
    public void SetHovered(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }
}
