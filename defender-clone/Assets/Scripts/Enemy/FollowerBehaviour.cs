using UnityEngine;

public class FollowerBehaviour : EnemyBehaviour
{
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
            else
            {
                MoveTowardsPosition(player.position);
            }
        }
    }
}