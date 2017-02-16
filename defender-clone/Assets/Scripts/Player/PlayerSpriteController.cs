using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private bool blinking = false;
    public bool Blinking
    {
        get
        {
            return blinking;
        }
        set
        {
            blinking = value;
            if (blinking)
            {
                StartCoroutine(Blink());
            }
            else
            {
                StopCoroutine(Blink());
            }
        }
    }
    public bool flipX
    {
        get
        {
            return sprite.flipX;
        }
        set
        {
            sprite.flipX = value;
        }
    }
    private Color mainColor;
    public Color[] blinkColors = { Color.black, Color.white };
    public int flashFrames = 2;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        mainColor = sprite.color;
    }

    private IEnumerator Blink()
    {
        int index = 0;
        while (blinking)
        {
            for (int i = 0; i < flashFrames; i++)
            {
                yield return null;
            }
            sprite.color = blinkColors[index % blinkColors.Length];
            index++;
        }
        sprite.color = mainColor;
    }
}
