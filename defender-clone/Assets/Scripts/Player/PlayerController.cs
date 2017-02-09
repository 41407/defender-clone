using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
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

    void Awake()
    {
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

    void Update()
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
        float clampTranslate = firing ? maxVelocity * firingVelocityModifier : maxVelocity;
        if (Vector2.Distance(targetPosition, transform.position) > 0.25f)
        {
            if (targetPosition.x < transform.position.x)
            {
                direction = Vector2.left;
                sprite.flipX = true;
            }
            else
            {
                direction = Vector2.right;
                sprite.flipX = false;
            }
        }
        transform.Translate(Vector2.ClampMagnitude(new Vector2(targetPosition.x - transform.position.x, 0), clampTranslate) * Time.deltaTime);
    }

    private void TranslateY()
    {
        body.velocity = Vector2.ClampMagnitude(new Vector2(0, targetPosition.y - transform.position.y), 2) * 8;
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
            targetPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else if (!moving)
        {
            moving = true;
            movingTouchId = heldTouchId;
            targetPosition = Camera.main.ScreenToWorldPoint(position);
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
        int frame = 0;
        Color[] colors = { Color.black, Color.black, Color.white, Color.white };
        for (float time = 0; time < 2; time += Time.deltaTime)
        {
            frame++;
            sprite.color = colors[frame % colors.Length];
            yield return null;
        }
        sprite.color = Color.white;
        invulnerable = false;
    }
}
