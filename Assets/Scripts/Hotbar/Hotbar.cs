using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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

    public void OnNumberKeyPress(InputAction.CallbackContext ctx)
    {
        var key = int.Parse(ctx.control.name);

        switch (key)
        {
            case 1:
                Debug.Log("User pressed number 1");
                break;
            
            case 2:
                Debug.Log("User pressed number 2");
                break;
            
            case 3:
                Debug.Log("User pressed number 3");
                break;
            
            case 4:
                Debug.Log("User pressed number 4");
                break;
            
            case 5:
                Debug.Log("User pressed number 5");
                break;
            
            case 6:
                Debug.Log("User pressed number 6");
                break;
            
            case 7:
                Debug.Log("User pressed number 7");
                break;
            
            case 8:
                Debug.Log("User pressed number 8");
                break;
            
            case 9:
                Debug.Log("User pressed number 9");
                break;
            
            default:
                Debug.Log("Invalid key pressed");
                break;
        }
    }
}
