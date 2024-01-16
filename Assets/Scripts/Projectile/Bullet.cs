using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] private int _lifetime; // in seconds

    void Start()
    {
        Debug.Assert(_lifetime > 0);
        Debug.Assert(damage >= 0);
        
        StartCoroutine(DespawnAfterSeconds(_lifetime));
    }

    // Despawns the bullet after `_lifetime` seconds has passed
    IEnumerator DespawnAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // Called by other gameobjects when this bullet hits them
    public void HandleHit()
    {
        Destroy(gameObject);
    }
}
