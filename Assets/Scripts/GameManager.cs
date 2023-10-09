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
    // Possible rename to PerformActionOnFocusedTile(...){} b/c hotbar may contain weapons
    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null)
        {
            return;
        }

        var hotbar = player.hotbar;
        var selectedAction = hotbar.selectedAction;

        if (selectedAction.actionType == Action.ActionType.Empty)
        {
            // Do nothing
        }
        else if (selectedAction.actionType == Action.ActionType.Build)
        {
            var action = selectedAction as BuildAction;
            var selectedSprite = action!.selectedSprite;
            //focusedTile.SetSprite(selectedSprite);
            var adjacentTiles = focusedTile.adjacentTiles;
            var focusedTileAdjacentToPlayer = adjacentTiles.Find(
                tile => tile.isBeingWalkedOn);

            if (focusedTileAdjacentToPlayer != null)
            {
                focusedTile.SetSprite(selectedSprite);
            }
            else
            {
                Debug.Log("Attempted to build on a tile not adjacent to the player");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
