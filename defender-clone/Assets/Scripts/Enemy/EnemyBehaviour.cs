using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    private GameController gameController;
    public enum EnemyBehaviourType
    {
        follower,
        shooter
    }
    public GameObject astronaut;
    private bool hasAttemptedAbduction = false;
    public EnemyBehaviourType enemyBehaviourType;
    private Transform player;
    public float speed = 3;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
    }

    void OnEnable()
    {
        speed = Mathf.Clamp(gameController.difficulty / 8, 3, 6);
        astronaut = null;
        hasAttemptedAbduction = false;
        switch (enemyBehaviourType)
        {
            case EnemyBehaviourType.follower:
                StartCoroutine(FollowerBehaviour());
                break;
            case EnemyBehaviourType.shooter:
                StartCoroutine(ShooterBehaviour());
                break;
            default:
                StartCoroutine(FollowerBehaviour());
                break;
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

    private IEnumerator FollowerBehaviour()
    {
        while (true)
        {
            if (player == null)
            {
                FindPlayer();
            }
            else
            {
                transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
                if (!hasAttemptedAbduction)
                {
                    yield return AttemptAbduction();
                }
            }
            yield return null;
        }
    }


    private IEnumerator ShooterBehaviour()
    {
        while (true)
        {
            if (player == null)
            {
                FindPlayer();
            }
            else
            {
                if (Vector2.Distance(player.position, transform.position) > 5)
                {
                    transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector2(transform.position.x - player.position.x, 0).normalized * speed * Time.deltaTime);
                    GetComponent<EnemyFiring>().FireBurst(player.position - transform.position, Mathf.Clamp(gameController.difficulty / 5, 3, 10));
                }
                if (!hasAttemptedAbduction)
                {
                    yield return AttemptAbduction();
                }
            }
            yield return null;
        }
    }

    private IEnumerator AttemptAbduction()
    {
        if (Vector2.Distance(player.position, transform.position) > 20)
        {
            hasAttemptedAbduction = true;
            astronaut = FindClosestAstronaut(transform.position);
            AstronautController ac = astronaut.GetComponent<AstronautController>();
            if (!ac.beingAbducted && Vector2.Distance(transform.position, astronaut.transform.position) < 20)
            {
                ac.beingAbducted = true;
                yield return Abduct();
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator Abduct()
    {
        while (astronaut.activeInHierarchy)
        {
            if (astronaut.GetComponent<AstronautController>().abductor == gameObject)
            {
                transform.Translate(Vector2.up * 1.5f * Time.deltaTime);
            }
            else
            {
                transform.Translate((astronaut.transform.position - transform.position).normalized * speed * Time.deltaTime);
            }
            yield return null;
        }
    }
    private GameObject FindClosestAstronaut(Vector2 position)
    {
        GameObject[] astronauts = GameObject.FindGameObjectsWithTag("Astronaut");
        return astronauts.First(closest => Vector2.Distance(position, closest.transform.position) == (astronauts.Min(astronaut => Vector2.Distance(position, astronaut.transform.position))));
    }
}
