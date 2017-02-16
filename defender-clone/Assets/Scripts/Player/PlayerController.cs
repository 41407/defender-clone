using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody2D body;
    private PlayerSpriteController playerSpriteController;
    private SpriteRenderer sprite;
    private int movingTouchId = -1;
    private Vector2 inputPosition;
    private bool invulnerable = false;
    private Vector2 velocity;
    public GameObject explosionParticlePrefab;
    public bool thrusting = false;
    public Vector2 direction = Vector2.right;
    public Vector2 maxVelocity = new Vector2(16, 5);
    public float xTranslateDeadzone = 0.1f;
    public Vector2 acceleration = new Vector2(0.2f, 1);
    public float xBrakingMultiplier = 2;
    public Vector2 deceleration = new Vector2(0.98f, 0.7f);

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        playerSpriteController = GetComponentInChildren<PlayerSpriteController>();
        sprite = transform.GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EventManager.OnButtonUp += StopThrusting;
        EventManager.OnButtonHold += UpdateInputPosition;
        EventManager.OnButtonDown += StartThrusting;
        StartCoroutine(SetSpawnInvulnerability());
    }

    void FixedUpdate()
    {
        AccelerateX();
        AccelerateY();
        if (!thrusting)
        {
            velocity.x *= deceleration.x;
        }
        velocity.y *= deceleration.y;
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

    private void AccelerateX()
    {
        if (thrusting && Mathf.Abs(inputPosition.x) > xTranslateDeadzone)
        {
            if (inputPosition.x < 0)
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
            currentAcceleration *= Mathf.Sign(direction.x) == Mathf.Sign(velocity.x) ? 1 : xBrakingMultiplier;
            velocity.x = Mathf.Clamp(velocity.x + direction.x * currentAcceleration, -maxVelocity.x, maxVelocity.x);
        }
    }

    private void AccelerateY()
    {
        if (!thrusting)
        {
            return;
        }
        float targetPositionY = inputPosition.y * gameController.levelHeight - body.position.y;
        velocity.y = Mathf.Clamp(velocity.y + targetPositionY * acceleration.y, -maxVelocity.y, maxVelocity.y);
    }

    void OnDisable()
    {
        EventManager.OnButtonUp -= StopThrusting;
        EventManager.OnButtonHold -= UpdateInputPosition;
        EventManager.OnButtonDown -= StartThrusting;
    }

    private void StartThrusting(Vector2 position, int startedTouchId)
    {
        thrusting = true;
        movingTouchId = startedTouchId;
        inputPosition = position;
    }

    private void UpdateInputPosition(Vector2 position, int heldTouchId)
    {
        inputPosition = position;
        if (!thrusting)
        {
            thrusting = true;
            movingTouchId = heldTouchId;
            inputPosition = position;
        }
    }

    private void StopThrusting(Vector2 position, int releasedTouchId)
    {
        if (releasedTouchId == movingTouchId)
        {
            thrusting = false;
            inputPosition = position;
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
