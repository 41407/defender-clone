using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    public float xAcceleration = 0.2f;
    public float xBrakingMultiplier = 2;
    public float xDeceleration = 0.95f;

    void Awake()
    {
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
            float acceleration = xAcceleration;
            acceleration *= Mathf.Sign(targetPosition.x) == Mathf.Sign(direction.x) ? 1 : xBrakingMultiplier;
            velocity += direction * acceleration;
        }
        else
        {
            velocity *= xDeceleration;
        }
        body.velocity = velocity;
    }

    private void TranslateY()
    {
        //body.velocity = Vector2.ClampMagnitude(new Vector2(0, targetPosition.y - transform.position.y), 2) * 8;
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
