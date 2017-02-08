using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool moving = false;
    private bool firing = false;
    private int movingTouchId = -1;
    private int firingTouchId = -1;
    private Vector2 targetPosition;
    public float maxVelocity = 10;
    public float firingVelocityModifier = 0.4f;

    void OnEnable()
    {
        EventManager.OnButtonUp += StopMovingOrFiring;
        EventManager.OnButtonHold += UpdateTargetPosition;
        EventManager.OnButtonDown += StartMovingOrFiring;
    }

    void Update()
    {
        TranslateX();
        TranslateY();
    }

    private void TranslateX()
    {
        float clampTranslate = firing ? maxVelocity * firingVelocityModifier : maxVelocity;
        transform.Translate(Vector2.ClampMagnitude(new Vector2(targetPosition.x - transform.position.x, 0), clampTranslate) * Time.deltaTime);
    }

    private void TranslateY()
    {
        transform.Translate(Vector2.ClampMagnitude(new Vector2(0, targetPosition.y - transform.position.y), 2) * 10 * Time.deltaTime);
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
            targetPosition = transform.position;
        }
        else if (releasedTouchId == firingTouchId)
        {
            firing = false;
        }
    }
}
