using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    // Tile components
    public SpriteRenderer sr;
    private bool playerMouseIsOverTile;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sr != null);
    }

    public bool PlayerMouseIsOverTile()
    {
        return playerMouseIsOverTile;
    }

    private void OnMouseEnter()
    {
        playerMouseIsOverTile = true;
    }

    private void OnMouseExit()
    {
        playerMouseIsOverTile = false;
    }
}
