using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explosionParticlePrefab;
    public float timeToLive = 2;
    private float currentTime = 0;

    void OnEnable()
    {
        currentTime = 0;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime >= timeToLive)
        {
            Disable();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Disable();
    }

    private void Disable()
    {
        if (explosionParticlePrefab)
        {
            Factory.create.ByReference(explosionParticlePrefab, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}
