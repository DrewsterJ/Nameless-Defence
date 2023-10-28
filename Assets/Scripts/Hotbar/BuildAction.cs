using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Building Types: Turrets, Walls, Resource Storage Depots, Resource Gathering Facilities

public class BuildAction : Action
{
    private enum Position
    {
        One,
        Two,
        Three,
        Four,
        Five
    }
    
    // UI Components
    private VisualElement buildMenuRootElement;
    public UIDocument uiDocument;
    public Sprite selectedSprite;
    public GameObject selectedBuilding;
    
    // Turret sprite images
    public List<GameObject> turretBuildings;

    // Contains buttons for selecting buildings in the build menu
    private Dictionary<Position, Dictionary<Position, Button>> selectableBuildingButtons;
    
    void Start()
    {
        Debug.Assert(uiDocument != null);
        actionType = ActionType.Build;
        SetupUI();
    }

    // Sets up UI components and links them with their callback functions
    private void SetupUI()
    {
        // Setup root visual element
        buildMenuRootElement = uiDocument.rootVisualElement.Query<VisualElement>("build-menu-background");
        
        // Setup other visual elements
        selectableBuildingButtons = new Dictionary<Position, Dictionary<Position, Button>>()
        {
            { Position.One, BuildSelectableButtonRow("rowOne") },
            { Position.Two, BuildSelectableButtonRow("rowTwo") },
            { Position.Three, BuildSelectableButtonRow("rowThree") },
            { Position.Four, BuildSelectableButtonRow("rowFour") },
            { Position.Five, BuildSelectableButtonRow("rowFive") }
        };
        SetBuildMenuVisibility(false);
        
        // Set callback functions for UI elements
        selectableBuildingButtons[Position.One][Position.One].clicked += () => SetSelectedBuilding(turretBuildings[0]);
        foreach (var row in selectableBuildingButtons.Values)
        {
            foreach (var button in row)
            {
                Debug.Assert(button.Value != null);
                
                // If an icon has been assigned to this item
                if (button.Value.style.backgroundImage != null)
                    continue;
                
                button.Value.clicked += () => SetSelectedBuilding(null);
            }
        }
        
        selectableBuildingButtons[Position.One][Position.Two].clicked += () => SetSelectedBuilding(null);
        
        // Verify setup was successful
        Debug.Assert(buildMenuRootElement != null);
    }
    
    // Builds a dictionary of selectable buttons that represent buildings on the build menu
    private Dictionary<Position, Button> BuildSelectableButtonRow(string rowName)
    {
        var row = new Dictionary<Position, Button>()
        {
            { Position.One, GetRawUIComponent<Button>(rowName + "ItemOne") },
            { Position.Two, GetRawUIComponent<Button>(rowName + "ItemTwo") },
            { Position.Three, GetRawUIComponent<Button>(rowName + "ItemThree") },
            { Position.Four, GetRawUIComponent<Button>(rowName + "ItemFour") },
            { Position.Five, GetRawUIComponent<Button>(rowName + "ItemFive") }
        };

        return row;
    }

    // Returns the raw UI component of the given type and name from the BuildAction UIDocument
    private T GetRawUIComponent<T>(string componentName) where T : VisualElement
    {
        return (buildMenuRootElement.Query<T>(componentName));
    }
    
    // Returns whether the build menu is visible on the screen
    public bool GetBuildMenuVisibility()
    {
        return buildMenuRootElement.visible;
    }

    // Sets the visibility of the build menu
    public void SetBuildMenuVisibility(bool visibility)
    {
        if (visibility == buildMenuRootElement.visible)
            return;

        buildMenuRootElement.visible = visibility;
        SetChildrenVisibility(buildMenuRootElement, visibility);
    }

    // Sets the building that the player will place down during a build action
    private void SetSelectedBuilding(GameObject building)
    {
        selectedBuilding = building;
    }

    // Recursively sets all children of the given element to the given visibility
    private void SetChildrenVisibility(VisualElement elem, bool visibility)
    {
        Debug.Assert(elem != null);
        
        foreach (var child in elem.Children())
        {
            child.visible = visibility;
            if (child.childCount > 0)
                SetChildrenVisibility(child, visibility);
        }
    }
}
