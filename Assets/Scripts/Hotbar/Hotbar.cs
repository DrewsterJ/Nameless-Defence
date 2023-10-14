using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public List<Action> hotbarActions;
    public Action selectedAction;

    private void Start()
    {
        Debug.Assert(hotbarActions.Count > 0);
        
        selectedAction = hotbarActions[0];
    }
}
