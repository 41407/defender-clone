using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletVelocity;
    private PlayerController playerController;
    public float rateOfFire = 0.16f;

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
            if (playerController.firing)
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
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
    }
}
