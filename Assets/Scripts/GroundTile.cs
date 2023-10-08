using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sr != null);
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
}
