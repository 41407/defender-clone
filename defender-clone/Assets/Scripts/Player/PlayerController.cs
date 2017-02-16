using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody2D body;
    private PlayerSpriteController playerSpriteController;
    public bool moving = false;
    public bool firing = false;
    private int movingTouchId = -1;
    private int firingTouchId = -1;
    private Vector2 targetPosition;
    public float maxVelocity = 10;
    public float firingVelocityModifier = 0.4f;
    private SpriteRenderer sprite;
    public Vector2 direction;
    private bool invulnerable = false;
    public GameObject explosionParticlePrefab;
    public float xTranslateDeadzone = 0.1f;
    private Vector2 velocity;
    public Vector2 acceleration = new Vector2(0.2f, 1);
    public float xBrakingMultiplier = 2;
    public Vector2 deceleration = new Vector2(0.98f, 0.7f);
    public float maxYTranslateStep = 0.5f;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        playerSpriteController = GetComponentInChildren<PlayerSpriteController>();
        sprite = transform.GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EventManager.OnButtonUp += StopMovingOrFiring;
        EventManager.OnButtonHold += UpdateTargetPosition;
        EventManager.OnButtonDown += StartMovingOrFiring;
        targetPosition = transform.position;
        StartCoroutine(SetSpawnInvulnerability());
    }

    void FixedUpdate()
    {
        TranslateX();
        TranslateY();
        body.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!invulnerable)
        {
            if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || coll.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Bullet"))
            {
                Factory.create.ByReference(explosionParticlePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void TranslateX()
    {
        if (moving && Mathf.Abs(targetPosition.x) > xTranslateDeadzone)
        {
            if (targetPosition.x < 0)
            {
                direction = Vector2.left;
                sprite.flipX = true;
            }
            else
            {
                direction = Vector2.right;
                sprite.flipX = false;
            }
            float currentAcceleration = acceleration.x;
            currentAcceleration *= Mathf.Sign(targetPosition.x) == Mathf.Sign(direction.x) ? 1 : xBrakingMultiplier;
            velocity += direction * currentAcceleration;
        }
        else
        {
            velocity = new Vector2(velocity.x * deceleration.x, velocity.y);
        }
    }

    private void TranslateY()
    {
        if (!moving)
        {
            return;
        }
        float relativeYPosition = body.position.y / gameController.levelHeight - 0.5f;
        Vector2 targetWorldYPosition = new Vector2(body.position.x, targetPosition.y * gameController.levelHeight);
        if (Mathf.Abs(targetWorldYPosition.y) < maxYTranslateStep * 5)
        {
            body.position = new Vector2(body.position.x, Mathf.Lerp(body.position.y, targetWorldYPosition.y, 0.2f));
        }
        else
        {
            body.position = Vector2.MoveTowards(body.position, targetWorldYPosition, maxYTranslateStep);
        }
    }

    void OnDisable()
    {
        EventManager.OnButtonUp -= StopMovingOrFiring;
        EventManager.OnButtonHold -= UpdateTargetPosition;
        EventManager.OnButtonDown -= StartMovingOrFiring;
    }

    private void StartMovingOrFiring(Vector2 position, int startedTouchId)
    {
        if (!moving)
        {
            moving = true;
            movingTouchId = startedTouchId;
        }
        if (startedTouchId != movingTouchId && !firing)
        {
            firing = true;
            firingTouchId = startedTouchId;
        }
    }

    private void UpdateTargetPosition(Vector2 position, int heldTouchId)
    {
        if (heldTouchId == movingTouchId)
        {
            targetPosition = position;
        }
        else if (!moving)
        {
            moving = true;
            movingTouchId = heldTouchId;
            targetPosition = position;
        }
    }

    private void StopMovingOrFiring(Vector2 position, int releasedTouchId)
    {
        if (releasedTouchId == movingTouchId)
        {
            moving = false;
            firing = false;
            targetPosition = transform.position;
        }
        else if (releasedTouchId == firingTouchId)
        {
            firing = false;
        }
    }

    private IEnumerator SetSpawnInvulnerability()
    {
        invulnerable = true;
        playerSpriteController.Blinking = true;
        yield return new WaitForSeconds(2);
        playerSpriteController.Blinking = false;
        invulnerable = false;
    }
}
