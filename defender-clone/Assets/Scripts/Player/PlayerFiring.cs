using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletVelocity;
    private PlayerController playerController;
    public float rateOfFire = 0.16f;
    public float bulletSpawnOffset = 1;
    public float accuracy = 0.05f;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        StartCoroutine(FireCo());
    }

    private IEnumerator FireCo()
    {
        while (true)
        {
            if (playerController.moving)
            {
                Fire(playerController.direction);
                yield return new WaitForSeconds(rateOfFire);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void Fire(Vector2 direction)
    {
        direction += Random.insideUnitCircle * accuracy;
        GameObject newBullet = Factory.create.ByReference(bulletPrefab, transform.position + (Vector3)direction * bulletSpawnOffset, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
    }
}
