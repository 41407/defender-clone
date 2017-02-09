using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFiring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletVelocity;
    private bool onCooldown = false;
    public float accuracy = 0.05f;

    public void FireBurst(Vector2 direction, int count)
    {
        if (!onCooldown)
        {
            StartCoroutine(FireCo(direction, count));
        }
    }

    private IEnumerator FireCo(Vector2 direction, int count)
    {
        onCooldown = true;
        for (int i = 0; i < count; i++)
        {
            Fire(direction);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2);
        onCooldown = false;
    }

    private void Fire(Vector2 direction)
    {
        direction += Random.insideUnitCircle * accuracy;
        GameObject newBullet = Factory.create.ByReference(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
    }
}
