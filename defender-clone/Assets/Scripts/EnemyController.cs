using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject explosionParticlePrefab;
    public int scoreOnDestroy = 50;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player Bullet") || coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Factory.create.ByReference(explosionParticlePrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Score.score += scoreOnDestroy;
        }
    }
}
