using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] explosionPrefabs;
    public int scoreOnDestroy = 50;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player Bullet") || coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (explosionPrefabs.Length > 0)
            {
                for (int i = 0; i < explosionPrefabs.Length; i++)
                {
                    Factory.create.ByReference(explosionPrefabs[i], transform.position, Quaternion.identity);
                }
            }
            gameObject.SetActive(false);
            Score.score += scoreOnDestroy;
        }
    }
}
