using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody2D body;
    private PlayerSpriteController playerSpriteController;
    private PlayerThrusterParticles playerThrusterParticles;
    private int movingTouchId = -1;
    private Vector2 inputPosition;
    private bool invulnerable = false;
    public Vector2 velocity;
    public GameObject explosionParticlePrefab;
    public bool thrusting = false;
    public Vector2 direction = Vector2.right;
    public Vector2 maxVelocity = new Vector2(16, 5);
    public float xTranslateDeadzone = 0.1f;
    public Vector2 acceleration = new Vector2(0.2f, 1);
    public Vector2 brakingAccelerationMultiplier = new Vector2(2, 1);
    public Vector2 deceleration = new Vector2(0.98f, 0.7f);

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        playerSpriteController = GetComponentInChildren<PlayerSpriteController>();
        playerThrusterParticles = GetComponentInChildren<PlayerThrusterParticles>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EventManager.OnButtonUp += StopThrusting;
        EventManager.OnButtonHold += UpdateInputPosition;
        EventManager.OnButtonDown += StartThrusting;
        StartCoroutine(SetSpawnInvulnerability());
    }

    void OnDisable()
    {
        EventManager.OnButtonUp -= StopThrusting;
        EventManager.OnButtonHold -= UpdateInputPosition;
        EventManager.OnButtonDown -= StartThrusting;
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
                playerSpriteController.flipX = true;
            }
            else
            {
                direction = Vector2.right;
                playerSpriteController.flipX = false;
            }
            float currentAcceleration = acceleration.x;
            currentAcceleration *= Mathf.Sign(direction.x) == Mathf.Sign(velocity.x) ? 1 : brakingAccelerationMultiplier.x;
            velocity.x = Mathf.Clamp(velocity.x + direction.x * currentAcceleration, -maxVelocity.x, maxVelocity.x);
            playerThrusterParticles.Emit();
        }
        else if (thrusting)
        {
            playerThrusterParticles.Idle();
        }
    }

    private void AccelerateY()
    {
        if (!thrusting)
        {
            return;
        }
        float targetPositionY = inputPosition.y * gameController.levelHeight - body.position.y;
        float currentAcceleration = acceleration.y;
        currentAcceleration *= Mathf.Sign(targetPositionY) == Mathf.Sign(velocity.y) ? 1 : brakingAccelerationMultiplier.y;
        velocity.y = Mathf.Clamp(velocity.y + targetPositionY * currentAcceleration, -maxVelocity.y, maxVelocity.y);
    }


    private void StartThrusting(Vector2 inputEventPosition, int startedTouchId)
    {
        thrusting = true;
        movingTouchId = startedTouchId;
        inputPosition = inputEventPosition;
    }

    private void UpdateInputPosition(Vector2 inputEventPosition, int heldTouchId)
    {
        if (!thrusting)
        {
            thrusting = true;
            movingTouchId = heldTouchId;
        }
        if (heldTouchId == movingTouchId)
        {
            inputPosition = inputEventPosition;
        }
    }

    private void StopThrusting(Vector2 inputEventPosition, int releasedTouchId)
    {
        if (releasedTouchId == movingTouchId)
        {
            thrusting = false;
            inputPosition = inputEventPosition;
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
