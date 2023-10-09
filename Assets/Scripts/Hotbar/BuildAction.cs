using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildAction : Action
{
    // UI Menu
    // List of sprites that the user can build organized into sections (turrets, walls, resources, etc.)
    
    // Possible building types: Turrets, walls, resource storage depots, resource gathering facilities, etc.

    public UIDocument uiDocument;
    public VisualElement root;
    
    // Turret sprite images
    public List<Sprite> turretBuildings;

    public Sprite selectedSprite;

    public VisualElement background;
    public VisualElement rowOne;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(uiDocument != null);
        SetupUI();
        Debug.Assert(root != null);
        actionType = ActionType.Build;
    }

    private void SetupUI()
    {
        // TODO: We should probably name the item labels (rowOneItemOne, rowTwoItemTwo, etc.) to a more descriptive name of what it applies to
        root = uiDocument.rootVisualElement;

        Button rowOneItemOne = root.Query<Button>("rowOneItemOne");
        background = root.Query<VisualElement>("background");
        Debug.Assert(rowOneItemOne != null);
        rowOneItemOne.clicked += SetSelectedSprite;
        root.visible = false;
        background.visible = false;
        var objects = background.Children();
        rowOne = root.Query<VisualElement>("rowOne");
        rowOne.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedSprite()
    {
        Debug.Log("Got called!");
        selectedSprite = turretBuildings[0];
    }
}
