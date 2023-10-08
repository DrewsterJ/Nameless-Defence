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
    public List<GameObject> adjacentTiles = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sr != null);
        Debug.Assert(adjacentTiles != null);
        adjacentTiles = IdentifyAdjacentTiles();
    }

    private List<GameObject> IdentifyAdjacentTiles()
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

        List<GameObject> adjacentTiles = new List<GameObject>();
        if (leftTile.collider != null)
            adjacentTiles.Add(leftTile.collider.gameObject);
        
        if (rightTile.collider != null)
            adjacentTiles.Add(rightTile.collider.gameObject);
        
        if (upTile.collider != null)
            adjacentTiles.Add(upTile.collider.gameObject);
        
        if (downTile.collider != null)
            adjacentTiles.Add(downTile.collider.gameObject);

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
        Debug.Log($"Colliding with {other.gameObject.tag}");
    }
}
