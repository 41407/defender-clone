using UnityEngine;

public class ShooterBehaviour : EnemyBehaviour
{
    private EnemyFiring enemyFiring;

    protected override void Awake()
    {
        base.Awake();
        enemyFiring = GetComponent<EnemyFiring>();
    }

    void FixedUpdate()
    {
        if (abducting)
        {
            Abduct();
        }
        else if (player != null)
        {
            if (!hasAttemptedAbduction)
            {
                AttemptAbduction();
            }
            else if (Vector2.Distance(player.position, transform.position) > 5)
            {
                MoveTowardsPosition(player.position);
            }
            else
            {
                enemyFiring.FireBurst(player.position - transform.position, 4);
            }
        }
    }
}