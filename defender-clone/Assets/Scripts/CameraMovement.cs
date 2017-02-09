using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;
    private Transform player;
    private Vector2 targetPosition;
    private bool holding = false;
    private int mainTouchId;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        EventManager.OnButtonDown += AssignMainTouch;
        EventManager.OnButtonHold += SetTargetPosition;
        EventManager.OnButtonUp += ReleaseTouch;
    }

    void Update()
    {
        if (player == null)
        {
            try
            {
                player = Component.FindObjectOfType<PlayerController>().transform;
            }
            catch (System.NullReferenceException)
            {
                // wait
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0, -10);
        }
    }

    private void AssignMainTouch(Vector2 position, int touchId)
    {
        if (!holding)
        {
            holding = true;
            mainTouchId = touchId;
        }
    }

    private void ReleaseTouch(Vector2 position, int touchId)
    {
        if (touchId == mainTouchId)
        {
            holding = false;
        }
    }

    private void SetTargetPosition(Vector2 position, int touchId)
    {
        if (player != null)
        {
            if (touchId == mainTouchId)
            {
                targetPosition = Vector2.Lerp(player.position, cam.ScreenToWorldPoint(position), 0.3f);
            }
            else if (!holding)
            {
                holding = true;
                mainTouchId = touchId;
            }
        }
    }
}