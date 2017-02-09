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
        shooter,
        exploding
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
        speed = Mathf.Clamp(gameController.wave / 8, 3, 6);
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

    void OnDisable()
    {
        StopAllCoroutines();
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
                Vector3 playerPosition = player.position;
                if (!hasAttemptedAbduction)
                {
                    yield return AttemptAbduction();
                }
                transform.Translate((playerPosition - transform.position).normalized * speed * Time.deltaTime);
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
                Vector3 playerPosition = player.position;
                if (!hasAttemptedAbduction)
                {
                    yield return AttemptAbduction();
                }
                if (Vector2.Distance(playerPosition, transform.position) > 5)
                {
                    transform.Translate((playerPosition - transform.position).normalized * speed * Time.deltaTime);
                }
                else
                {
                    GetComponent<EnemyFiring>().FireBurst(playerPosition - transform.position, Mathf.Clamp(gameController.wave / 5, 3, 10));
                    if (gameController.wave > 6)
                    {
                        yield return StartCoroutine(MoveAlongYForDuration(1));
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator MoveAlongYForDuration(float duration)
    {
        Vector2 translation = new Vector2(0, Random.Range(-speed, speed));
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            if (transform.position.y > 4)
            {
                translation = new Vector2(0, -translation.y);
            }
            transform.Translate(translation * Time.deltaTime);
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
