using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBehaviour : MonoBehaviour
{
    private EnemyBehaviour enemyBehaviour;
    private bool hasAttemptedAbduction = false;
    private bool abducting = false;
    private GameObject astronaut;
    public float beamingSpeed = 1;

    void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void OnEnable()
    {
        hasAttemptedAbduction = false;
    }

    void FixedUpdate()
    {
        if (abducting)
        {
            Abduct();
        }
        else if (enemyBehaviour.player != null)
        {
            if (!hasAttemptedAbduction)
            {
                AttemptAbduction();
            }
            else
            {
                MoveTowardsPosition(enemyBehaviour.player.position);
            }
        }
    }

    private void AttemptAbduction()
    {
        hasAttemptedAbduction = true;
        astronaut = FindClosestAstronaut(transform.position);
        if (astronaut != null)
        {
            AstronautController ac = astronaut.GetComponent<AstronautController>();
            if (!ac.beingAbducted && Vector2.Distance(transform.position, astronaut.transform.position) < 20)
            {
                ac.beingAbducted = true;
                abducting = true;
            }
        }
    }

    private void Abduct()
    {
        if (!astronaut.activeInHierarchy)
        {
            astronaut = null;
            abducting = false;
            return;
        }
        if (astronaut.GetComponent<AstronautController>().abductor == gameObject)
        {
            transform.Translate(Vector2.up * beamingSpeed * Time.fixedDeltaTime);
        }
        else
        {
            MoveTowardsPosition(astronaut.transform.position);
        }
    }

    private void MoveTowardsPosition(Vector2 position)
    {
        transform.Translate((position - (Vector2)transform.position).normalized * enemyBehaviour.speed * Time.fixedDeltaTime);
    }

    private GameObject FindClosestAstronaut(Vector2 position)
    {
        GameObject[] astronauts = GameObject.FindGameObjectsWithTag("Astronaut");
        if (astronauts.Length > 0)
        {
            return astronauts.First(closest => Vector2.Distance(position, closest.transform.position) == (astronauts.Min(astronaut => Vector2.Distance(position, astronaut.transform.position))));
        }
        else
        {
            return null;
        }
    }
}