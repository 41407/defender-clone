using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MineLayerBehaviour : EnemyBehaviour
{
    private EnemyFiring enemyFiring;
    private Vector2 direction;
    private float timeSinceMine;
    public float mineLayingInterval = 1;

    protected override void Awake()
    {
        enemyFiring = GetComponent<EnemyFiring>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = Random.value > 0.5f ? Vector2.right : Vector2.left;
        timeSinceMine = 0;
    }

    void FixedUpdate()
    {
        timeSinceMine += Time.fixedDeltaTime;
        MoveTowardsPosition(body.position + direction);
        if (timeSinceMine >= mineLayingInterval)
        {
            direction.y = Random.Range(-0.5f, 0.5f);
            enemyFiring.FireBurst(Vector2.zero, 1);
            timeSinceMine = 0;
        }
        if (transform.position.y > 4)
        {
            direction.y = -1;
        }
        else if (body.position.y < -4)
        {
            direction.y = 1;
        }
    }
}
