using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnMouseEnter()
    {
        GameManager.instance.SetFocusedTile(this);
    }

    private void OnMouseExit()
    {
        GameManager.instance.SetFocusedTile(null);
    }
}
