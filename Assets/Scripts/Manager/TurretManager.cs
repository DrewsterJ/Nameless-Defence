using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager instance;
    
    public List<Turret> turrets;
    public GameObject turretPrefab;
    public float enemySearchInterval; // how often turrets search for new enemies
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Coroutine for assigning enemy targets to idle turrets
    public IEnumerator TurretTargetingCoroutine()
    {
        while (true)
        {
            foreach (var turret in turrets.Where(turret =>
                         !turret.IsEngagingTarget() && !turret.IsDestroyed() &&
                         !turret.IsUnityNull()))
            {
                foreach (var enemy in EnemyManager.instance.enemies.Where(enemy =>
                             !enemy.IsDestroyed() && !enemy.IsUnityNull()))
                {
                    turret.TryEngageTarget(enemy.gameObject);
                    if (turret.IsEngagingTarget())
                        break;
                }
            }

            EnemyManager.instance.RemoveInvalidEnemies();
            RemoveInvalidTurrets();
            yield return new WaitForSeconds(enemySearchInterval);
        }
    }

    // Removes turrets that no longer exist or were destroyed
    public void RemoveInvalidTurrets()
    {
        turrets.RemoveAll(turret =>
        {
            return (turret == null || turret.IsDestroyed() || turret.IsUnityNull());
        });
    }
    
    // Spawns a turret in the world at the given ground tile
    public void SpawnTurretAtTile(GroundTile tile)
    {
        Debug.Assert(tile != null && !tile.IsDestroyed() && !tile.IsUnityNull());
        
        var turret = Instantiate(turretPrefab, tile.transform.position, turretPrefab.transform.rotation);
        turrets.Add(turret.GetComponent<Turret>());
    }
}
