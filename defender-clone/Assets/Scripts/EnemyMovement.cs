using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
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
            MoveTowardsPlayer();
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
}
