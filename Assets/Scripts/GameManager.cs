using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // The tile the player is hovering their mouse over
    private GroundTile focusedTile;

    private List<Turret> turrets;
    // private List<Enemy> enemies;
    

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Called by the GroundTile script to set the tile that the player is currently hovering over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    public void PerformPlayerLeftClick(PlayerController player)
    {
        // If the user is attempting to build
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            InteractWithFocusedTile(player);
        }
    }

    public void PerformPlayerRightClick(PlayerController player)
    {
        // User is opening the build menu
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            var action = player.hotbar.GetActiveHotbarAction() as BuildAction;
            action!.SetBuildMenuVisibility(!action.GetBuildMenuVisibility());
        }
    }

    // Called by the player to interact with a tile using any hotbar action item
    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null) 
            return;
        
        var hotbar = player.hotbar;
        var selectedAction = hotbar.GetActiveHotbarAction();

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
        
        var distance = Vector2.Distance(action.transform.position, tileToBuildOn.transform.position);
        const float actionRange = 2.0f;

        // Build the actively selected building if we're in action range
        if (distance < actionRange)
        {
            var building = action.selectedBuilding;
            Instantiate(building, tileToBuildOn.transform);
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }
}
