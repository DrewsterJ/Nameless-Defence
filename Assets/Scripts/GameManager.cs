using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // The tile the player is hovering their mouse over
    private GroundTile focusedTile;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Called by the GroundTile script to set the tile that the player is currently hovering over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    // Called by the player to interact with a tile using any hotbar action item
    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null) 
            return;
        
        var hotbar = player.hotbar;
        var selectedAction = hotbar.selectedAction;

        if (selectedAction.actionType == Action.ActionType.Empty)
        {
            // Do nothing
        }
        else if (selectedAction.actionType == Action.ActionType.Build)
        {
            var action = selectedAction as BuildAction;
            PerformBuildActionOnTile(action, focusedTile);
        }
    }

    private void PerformBuildActionOnTile(BuildAction action, GroundTile tileToBuildOn)
    {
        Debug.Assert(action != null);
        Debug.Assert(tileToBuildOn != null);

        var buildingSprite = action.selectedSprite;
        var distance = Vector2.Distance(action.transform.position, tileToBuildOn.transform.position);
        const float actionRange = 2.0f;

        if (distance < actionRange)
            tileToBuildOn.SetSprite(buildingSprite);
        else if (distance > 6)
        {
            action.root.visible = !action.root.visible;
            action.background.visible = !action.background.visible;
            var objects = action.background.Children();
            foreach (var obj in objects)
            {
                obj.visible = !obj.visible;
            }
            action.rowOne.visible = !action.rowOne.visible;
        }
            
    }
    
    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }
}
