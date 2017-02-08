using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public string staticText = "";
    public Vector3 targetScale = Vector3.one;
    public float scoreUpdateScalingSmoothness = 0.5f;
    public int displayedScore = 0;
    private TextMesh text;

    void Awake()
    {
        text = GetComponent<TextMesh>();
    }

    void Update()
    {
        if (Score.score > displayedScore)
        {
            transform.localScale = transform.localScale + Vector3.one * (Score.score - displayedScore);
        }
        displayedScore = Score.score;
        text.text = staticText + " " + displayedScore;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scoreUpdateScalingSmoothness);
    }
}
