using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int _bulletLifetime; // in seconds

    void Start()
    {
        StartCoroutine(DieAfterSeconds());
    }
    
    void Update()
    { }

    IEnumerator DieAfterSeconds()
    {
        if (!gameObject.IsUnityNull() && !gameObject.IsDestroyed())
        {
            yield return new WaitForSeconds(_bulletLifetime);
            Destroy(gameObject);
        }
    }
}
