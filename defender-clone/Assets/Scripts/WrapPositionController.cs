using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapPositionController : MonoBehaviour
{
    private GameController gameController;
    private float levelWidth;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        levelWidth = gameController.levelWidth;
    }

    void Update()
    {
        float positionDelta = gameController.cam.transform.position.x - transform.position.x;
        if (Mathf.Abs(positionDelta) > levelWidth / 2)
        {
            transform.Translate(new Vector2(levelWidth * Mathf.Sign(positionDelta), 0));
        }
    }
}
