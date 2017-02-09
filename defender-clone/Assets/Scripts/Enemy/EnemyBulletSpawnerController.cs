using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawnerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int count;
    public float minVelocity = 1;
    public float maxVelocity = 1;

    void OnEnable()
    {
        float angleStep = 360 / count;
        for (int i = 0; i < count; i++)
        {
            GameObject bullet = Factory.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(i * angleStep, Vector3.forward) * Vector2.up * Random.Range(minVelocity, maxVelocity);
        }
    }
}
