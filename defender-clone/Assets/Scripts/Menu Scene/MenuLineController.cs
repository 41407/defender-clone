using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLineController : MonoBehaviour
{
    private Vector2 targetPosition;
    public bool appearFromRight = true;

    void Awake()
    {
        targetPosition = transform.position;
        float targetX = appearFromRight ? 16 : -16;
        transform.Translate(new Vector2(targetX, 0));
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
    }
}
