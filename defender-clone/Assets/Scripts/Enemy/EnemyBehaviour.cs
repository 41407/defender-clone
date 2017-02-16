using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected WaveController waveController;
    protected GameController gameController;
    protected Rigidbody2D body;
    protected bool hasAttemptedAbduction = false;
    protected bool abducting = false;
    public GameObject astronaut;
    public Transform player;
    public float speed = 3;
    public float beamingSpeed = 1;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        waveController = Component.FindObjectOfType<WaveController>();
        gameController = Component.FindObjectOfType<GameController>();
    }

    protected virtual void OnEnable()
    {
        speed = Mathf.Clamp(waveController.wave / 8, 3, 6);
        if (gameController.player != null)
        {
            player = gameController.player.transform;
        }
        astronaut = null;
        hasAttemptedAbduction = false;
        abducting = false;
    }

    protected virtual void AttemptAbduction()
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

    protected virtual void Abduct()
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

    protected virtual void MoveTowardsPosition(Vector2 position)
    {
        transform.Translate((position - (Vector2)transform.position).normalized * speed * Time.fixedDeltaTime);
    }

    protected virtual GameObject FindClosestAstronaut(Vector2 position)
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
