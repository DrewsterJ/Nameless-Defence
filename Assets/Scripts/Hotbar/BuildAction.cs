using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAction : Action
{
    // UI Menu
    // List of sprites that the user can build organized into sections (turrets, walls, resources, etc.)
    
    
    // Possible building types: Turrets, walls, resource storage depots, resource gathering facilities, etc.
    
    // Turret sprite images
    public List<Sprite> turretBuildings;

    public Sprite selectedSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        actionType = ActionType.Build;
        selectedSprite = turretBuildings[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
