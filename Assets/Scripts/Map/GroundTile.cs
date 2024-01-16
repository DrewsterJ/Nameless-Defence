using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public LayerMask tileLayerMask;
    public List<GroundTile> adjacentTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(adjacentTiles != null);
        adjacentTiles = new List<GroundTile>(IdentifyAdjacentTiles());
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
}
