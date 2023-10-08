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

    // Sets the tile the player is currently hovering their mouse over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null)
        {
            Debug.Log("Player tried to interact with a non-existent tile");
            return;
        }

        var hotbar = player.hotbar;
        var selectedAction = hotbar.selectedAction;

        if (selectedAction.actionType == Action.ActionType.Empty)
        {
            Debug.Log("Player interacted with a focused tile using the empty tool");
        }
        else if (selectedAction.actionType == Action.ActionType.Build)
        {
            Debug.Log("Player interacted with a focused tile using the build tool");
            var action = selectedAction as BuildAction;
            var selectedSprite = action!.selectedSprite;
            focusedTile.SetSprite(selectedSprite);
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
