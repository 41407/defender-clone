using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesDisplay : MonoBehaviour
{

    private TextMesh text;
    private GameController gameController;
    private string staticText;
    private int lives = 0;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        text = GetComponent<TextMesh>();
        staticText = text.text;
    }

    void Update()
    {
        if (lives != gameController.lives)
        {
            lives = Mathf.Max(0, gameController.lives);
            text.text = staticText + lives;
        }
    }
}
