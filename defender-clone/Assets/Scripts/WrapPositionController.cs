using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapPositionController : MonoBehaviour
{
    private GameController gameController;
    private float levelWidth;
    public float checkInterval = 1;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
    }

    void OnEnable()
    {
        levelWidth = gameController.levelWidth;
        StartCoroutine(TranslateIfOutsideLevelWidth());
    }

    private IEnumerator TranslateIfOutsideLevelWidth()
    {
        while (true)
        {
            float positionDelta = gameController.cam.transform.position.x - transform.position.x;
            float sign = Mathf.Sign(positionDelta);
            if (Mathf.Abs(positionDelta) > levelWidth / 2)
            {
                transform.Translate(new Vector2(levelWidth * sign, 0));
            }
            if (checkInterval > 0)
            {
                yield return new WaitForSeconds(checkInterval);
            }
            else
            {
                yield return null;
            }
        }
    }
}
