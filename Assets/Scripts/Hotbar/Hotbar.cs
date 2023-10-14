using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hotbar : MonoBehaviour
{
    public List<Action> hotbarActions;
    public Action selectedAction;
    public UIDocument uiDocument;

    private void Start()
    {
        Debug.Assert(hotbarActions.Count > 0);
        
        selectedAction = hotbarActions[0];
    }
}
