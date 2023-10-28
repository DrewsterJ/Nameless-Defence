using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Hotbar : MonoBehaviour
{
    public List<Action> hotbarActions;
    public int activeHotbarSlotNumber = 0;
    
    // UI Components
    private VisualElement hotbarRootElement;
    public UIDocument uiDocument;
    private List<VisualElement> hotbarUISlots;
    
    private void Start()
    {
        Debug.Assert(hotbarActions.Count > 0);
        Debug.Assert(uiDocument != null);

        SetupUI();
    }
    
    // Sets up UI components and links them with their callback functions
    private void SetupUI()
    {
        // Setup root visual element
        hotbarRootElement = uiDocument.rootVisualElement.Query<VisualElement>("hotbar-background");
        
        // Setup other visual elements
        hotbarUISlots = new List<VisualElement>()
        {
            GetRawUIComponent<VisualElement>("hotbarSlotOne"),
            GetRawUIComponent<VisualElement>("hotbarSlotTwo"),
            GetRawUIComponent<VisualElement>("hotbarSlotThree"),
            GetRawUIComponent<VisualElement>("hotbarSlotFour"),
            GetRawUIComponent<VisualElement>("hotbarSlotFive"),
            GetRawUIComponent<VisualElement>("hotbarSlotSix"),
            GetRawUIComponent<VisualElement>("hotbarSlotSeven")
        };
        
        activeHotbarSlotNumber = 0;
        SetHotbarUISlotActive(hotbarUISlots[activeHotbarSlotNumber]);
        
        // Verify setup was successful
        foreach (var slot in hotbarUISlots)
            Debug.Assert(slot != null);
        Debug.Assert(hotbarRootElement != null);
    }
    
    // Returns the raw UI component of the given type and name from the BuildAction UIDocument
    private T GetRawUIComponent<T>(string componentName) where T : VisualElement
    {
        return (hotbarRootElement.Query<T>(componentName));
    }
    
    public VisualElement GetActiveHotbarUISlot()
    {
        return hotbarUISlots[activeHotbarSlotNumber];
    }
    
    public Action GetActiveHotbarAction()
    {
        return hotbarActions[activeHotbarSlotNumber];
    }
    
    private void SetActiveHotbarSlot(int slotNumber)
    {
        Debug.Assert(slotNumber >= 0 && slotNumber <= 6);
        
        var prevHotbarUISlot = GetActiveHotbarUISlot();
        activeHotbarSlotNumber = slotNumber;
        var newHotbarUISlot = GetActiveHotbarUISlot();
            
        SetHotbarUISlotInactive(prevHotbarUISlot);
        SetHotbarUISlotActive(newHotbarUISlot);
    }

    // Modifies the border of the given Hotbar UI Slot visual element to be 0 pixels wide and transparent
    private void SetHotbarUISlotInactive(VisualElement hotbarUISlot)
    {
        Debug.Assert(hotbarUISlot != null);
        
        var border = hotbarUISlot.Query<VisualElement>("hotbarSlotBorder");
        SetUIElementBorderColor(border, Color.clear);
        SetUIElementBorderWidth(border, 0);
    }

    // Modifies the border of the given Hotbar UI Slot visual element to be 3 pixels wide and white
    private void SetHotbarUISlotActive(VisualElement hotbarUISlot)
    {
        Debug.Assert(hotbarUISlot != null);
        
        var border = hotbarUISlot.Query<VisualElement>("hotbarSlotBorder");
        SetUIElementBorderColor(border, Color.white);
        SetUIElementBorderWidth(border, 3);
    }

    // Sets the given UI Element's border color to the given color
    private void SetUIElementBorderColor(VisualElement elem, StyleColor color)
    {
        Debug.Assert(elem != null);
        
        elem.style.borderTopColor = color;
        elem.style.borderBottomColor = color;
        elem.style.borderLeftColor = color;
        elem.style.borderRightColor = color;
    }

    // Sets the given UI Element's border width to the given width
    private void SetUIElementBorderWidth(VisualElement elem, int width)
    {
        Debug.Assert(elem != null);
        Debug.Assert(width >= 0);
        
        elem.style.borderTopWidth = width;
        elem.style.borderBottomWidth = width;
        elem.style.borderLeftWidth = width;
        elem.style.borderRightWidth = width;
    }

    // Handles keyboard input when keys 1 - 9 are pressed
    public void OnNumberKeyPress(InputAction.CallbackContext ctx)
    {
        // Obtain the actual number key (1 - 9)
        var rawNumberKey = int.Parse(ctx.control.name);
        
        if (rawNumberKey is < 1 or > 7)
            return;
        
        // Format the number for use within containers (where access starts at 0)
        var formattedNumberKey = rawNumberKey - 1;
        
        SetActiveHotbarSlot(formattedNumberKey);
    }
}
