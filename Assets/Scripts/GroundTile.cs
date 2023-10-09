using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;

public class GroundTile : MonoBehaviour
{
    public GridLayout grid;
    public SpriteRenderer sr;
    public LayerMask tileLayerMask;
    public List<GroundTile> adjacentTiles = new List<GroundTile>();

    public bool isBeingWalkedOn;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sr != null);
        Debug.Assert(adjacentTiles != null);
        adjacentTiles = IdentifyAdjacentTiles();
    }

    private List<GroundTile> IdentifyAdjacentTiles()
    {
        var position = transform.position;
        var leftTile = Physics2D.Raycast((Vector2)position + Vector2.left,
            Vector2.up, .10f, tileLayerMask);
        
        var rightTile = Physics2D.Raycast((Vector2)position + Vector2.right,
            Vector2.up, .10f, tileLayerMask);
        
        var upTile = Physics2D.Raycast((Vector2)position + Vector2.up,
            Vector2.up, .10f, tileLayerMask);
        
        var downTile = Physics2D.Raycast((Vector2)position + Vector2.down,
            Vector2.up, .10f, tileLayerMask);

        List<GroundTile> adjacentTiles = new List<GroundTile>();
        if (leftTile.collider != null)
            adjacentTiles.Add(leftTile.collider.gameObject.GetComponent<GroundTile>());
        
        if (rightTile.collider != null)
            adjacentTiles.Add(rightTile.collider.gameObject.GetComponent<GroundTile>());
        
        if (upTile.collider != null)
            adjacentTiles.Add(upTile.collider.gameObject.GetComponent<GroundTile>());
        
        if (downTile.collider != null)
            adjacentTiles.Add(downTile.collider.gameObject.GetComponent<GroundTile>());

        Debug.Assert(adjacentTiles.Count >= 2);
        
        return adjacentTiles;
    }

    private void OnMouseEnter()
    {
        GameManager.instance.SetFocusedTile(this);
    }

    private void OnMouseExit()
    {
        GameManager.instance.SetFocusedTile(null);
    }

    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isBeingWalkedOn = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isBeingWalkedOn = false;
    }
}
