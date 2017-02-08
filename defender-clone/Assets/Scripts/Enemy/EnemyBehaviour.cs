using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    private GameController gameController;
    public enum EnemyType
    {
        follower,
        shooter
    }
    public GameObject astronaut;
    private bool hasAttemptedAbduction = false;
    public EnemyType enemyType;
    private Transform player;
    public float speed = 3;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
    }

    void OnEnable()
    {
        astronaut = null;
        hasAttemptedAbduction = false;
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }
        else if (astronaut == null)
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
        else
        {
            Abduct();
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
        if (Vector2.Distance(player.position, transform.position) > 20 && !hasAttemptedAbduction)
        {
            hasAttemptedAbduction = true;
            astronaut = FindClosestAstronaut(transform.position);
            AstronautController ac = astronaut.GetComponent<AstronautController>();
            if (ac.beingAbducted)
            {
                astronaut = null;
            }
            else
            {
                ac.beingAbducted = true;
            }
        }
        else
        {
            transform.Translate((player.position - transform.position).normalized * speed * Time.deltaTime);
        }
    }

    private void Abduct()
    {
        if (astronaut.GetComponent<AstronautController>().abductor == transform)
        {
            transform.Translate(Vector2.up * 1 * Time.deltaTime);
        }
        else
        {
            transform.Translate((astronaut.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
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
            GetComponent<EnemyFiring>().FireBurst(player.position - transform.position, Mathf.Clamp(gameController.difficulty / 10, 3, 10));
        }
    }

    private GameObject FindClosestAstronaut(Vector2 position)
    {
        GameObject[] astronauts = GameObject.FindGameObjectsWithTag("Astronaut");
        return astronauts.First(closest => Vector2.Distance(position, closest.transform.position) == (astronauts.Min(astronaut => Vector2.Distance(position, astronaut.transform.position))));
    }
}
