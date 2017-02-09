using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color mainColor;
    public Color damageColor;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        mainColor = sprite.color;
    }

    void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        sprite.color = mainColor;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageCo());
        StartCoroutine(Shake());
    }

    private IEnumerator DamageCo()
    {
        sprite.color = damageColor;
        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }
        sprite.color = mainColor;
    }

    private IEnumerator Shake()
    {
        for (float time = 0; time < shakeDuration; time += Time.deltaTime)
        {
            transform.localPosition = Random.insideUnitCircle.normalized * (shakeDuration - time) * shakeMagnitude;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
}
