using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // The tile the player is hovering their mouse over
    private GroundTile focusedTile;

    public float _turretTargetAcquisitionInterval = 1;
    public float _enemyTargetAcquisitionInterval = 1;

    public List<Enemy> enemies;
    public List<Turret> turrets;

    private bool _gameActive;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void AddTurret(Turret turret)
    {
        if (turret == null)
            return;
        
        turrets.Add(turret);
    }
    
    public void RemoveTurret(Turret turret)
    {
        if (turret == null)
            return;

        turrets.Remove(turret);
    }

    public void AddEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;
        
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        enemies.Remove(enemy);
    }

    // Called by the GroundTile script to set the tile that the player is currently hovering over
    public void SetFocusedTile(GroundTile tile)
    {
        focusedTile = tile;
    }

    public void PerformPlayerLeftClick(PlayerController player)
    {
        // If the user is attempting to build
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            InteractWithFocusedTile(player);
        }
    }

    public void PerformPlayerRightClick(PlayerController player)
    {
        // User is opening the build menu
        if (player.hotbar.GetActiveHotbarAction().actionType == Action.ActionType.Build)
        {
            var action = player.hotbar.GetActiveHotbarAction() as BuildAction;
            action!.SetBuildMenuVisibility(!action.GetBuildMenuVisibility());
        }
    }

    // Called by the player to interact with a tile using any hotbar action item
    public void InteractWithFocusedTile(PlayerController player)
    {
        // There is no focused tile
        if (focusedTile == null) 
            return;
        
        var hotbar = player.hotbar;
        var selectedAction = hotbar.GetActiveHotbarAction();

        if (selectedAction.actionType == Action.ActionType.Empty)
        {
            // Do nothing
        }
        else if (selectedAction.actionType == Action.ActionType.Build)
        {
            var action = selectedAction as BuildAction;
            PerformBuildActionOnTile(action, focusedTile);
        }
    }

    private void PerformBuildActionOnTile(BuildAction action, GroundTile tileToBuildOn)
    {
        Debug.Assert(action != null);
        Debug.Assert(tileToBuildOn != null);
        
        var distance = Vector2.Distance(action.transform.position, tileToBuildOn.transform.position);
        const float actionRange = 2.0f;

        // Build the actively selected building if we're in action range
        if (distance < actionRange)
        {
            var building = action.selectedBuilding;
            Instantiate(building, tileToBuildOn.transform);
        }
    }
    
    IEnumerator TurretTargetingCoroutine()
    {
        while (_gameActive)
        {
            foreach (var turret in turrets.Where(turret => !turret.IsEngagingTarget()))
            {
                foreach (var enemy in enemies)
                {
                    turret.TryEngageTarget(enemy.gameObject);
                    if (turret.IsEngagingTarget())
                        break;
                }
            }
            
            yield return new WaitForSeconds(_turretTargetAcquisitionInterval);
        }
    }
    
    IEnumerator EnemyTargetingCoroutine()
    {
        while (_gameActive)
        {
            // Identify attackable targets on map (walls, turrets, player)
            // Identify unengaged enemies
            // For every unengaged enemy
            // - Find an ideal target (based on distance, type of target, etc.)
            // - Enemy engages that target
            yield return new WaitForSeconds(_enemyTargetAcquisitionInterval);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _gameActive = true;

        StartCoroutine(EnemyTargetingCoroutine());
        StartCoroutine(TurretTargetingCoroutine());
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
