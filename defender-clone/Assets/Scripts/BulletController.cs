using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explosionParticlePrefab;
    public float timeToLive = 2;

    void OnEnable()
    {
        StartCoroutine(DisableAfterTime(timeToLive));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        StopCoroutine(DisableAfterTime(timeToLive));
        Disable();
    }

    private IEnumerator DisableAfterTime(float ttl)
    {
        yield return new WaitForSeconds(ttl);
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
