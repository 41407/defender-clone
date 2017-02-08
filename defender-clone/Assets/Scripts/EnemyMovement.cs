using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType
    {
        follower,
        shooter
    }
    public EnemyType enemyType;
    private Transform player;
    public float speed = 3;

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }
        else
        {
            switch (enemyType)
            {
                case EnemyType.follower:
                    MoveTowardsPlayer();
                    break;
                case EnemyType.shooter:
                    LurkAroundPlayer();
                    break;
                default:
                    MoveTowardsPlayer();
                    break;
            }
        }
    }

    private void FindPlayer()
    {
        PlayerController playerController = Component.FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
    }

    private void LurkAroundPlayer()
    {
        if (Vector2.Distance(player.position, transform.position) > 5)
        {
            transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector2(transform.position.x - player.position.x, 0).normalized * speed * Time.deltaTime);
            GetComponent<EnemyFiring>().FireBurst(player.position - transform.position, 3);
        }
    }
}
