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
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sr != null);
        Debug.Assert(adjacentTiles != null);
        adjacentTiles = IdentifyAdjacentTiles();
    }

    // Shoots raycasts in all directions from this tile to find it's adjacent tiles
    private List<GroundTile> IdentifyAdjacentTiles()
    {
        Vector2 position = transform.position;
        var directions = new[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down};
        foreach (var direction in directions)
        {
            var hit = Physics2D.Raycast(position + direction, Vector2.up, .10f, tileLayerMask);
            if (hit.collider != null)
                adjacentTiles.Add(hit.collider.gameObject.GetComponent<GroundTile>());
        }
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
        if (sprite == null)
        {
            Debug.Log("Tile was null!");
            return;
        }
            
        
        sr.sprite = sprite;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { }

    private void OnTriggerExit2D(Collider2D other)
    { }
}
