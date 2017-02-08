using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    private Vector2 targetPosition;

    void Awake()
    {
        player = Component.FindObjectOfType<PlayerMovement>().transform;
    }

    void OnEnable()
    {
        EventManager.OnButtonHold += SetTargetPosition;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0, -10);
    }

    private void SetTargetPosition(Vector2 position, int touchId)
    {
        targetPosition = Vector2.Lerp(player.position, Camera.main.ScreenToWorldPoint(position), 0.3f);
    }
}