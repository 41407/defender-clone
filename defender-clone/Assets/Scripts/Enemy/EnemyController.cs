using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemySpriteController spriteController;
    public GameObject[] explosionPrefabs;
    public int scoreOnDestroy = 50;
    public int maxHealth = 1;
    private int health = 1;

    void Awake()
    {
        spriteController = GetComponentInChildren<EnemySpriteController>();
    }

    void OnEnable()
    {
        health = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player Bullet"))
        {
            health--;

        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            health = 0;
        }
        else
        {
            return;
        }
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Explode();
        }
        else
        {
            spriteController.TakeDamage();
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
