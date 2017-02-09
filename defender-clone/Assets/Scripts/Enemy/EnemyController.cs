using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] explosionPrefabs;
    public int scoreOnDestroy = 50;
    public int maxHealth = 1;
    private int health = 1;

    void OnEnable()
    {
        health = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player Bullet") || coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            health--;
            if (health <= 0)
            {
                Explode();
            }
        }
    }

    private void Explode()
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
