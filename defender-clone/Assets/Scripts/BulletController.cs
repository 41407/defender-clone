using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float timeToLive = 2;

    void OnEnable()
    {
        StartCoroutine(DisableAfterTime(timeToLive));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        StopCoroutine(DisableAfterTime(timeToLive));
        gameObject.SetActive(false);
    }

    private IEnumerator DisableAfterTime(float ttl)
    {
        yield return new WaitForSeconds(ttl);
        gameObject.SetActive(false);
    }
}
