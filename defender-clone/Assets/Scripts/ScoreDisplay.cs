using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public string staticText = "";
    public int displayedScore = 0;
    private TextMesh text;
    private float shake;
    private Vector3 anchorPosition;

    void Awake()
    {
        text = GetComponent<TextMesh>();
        anchorPosition = transform.localPosition;
    }

    void Update()
    {
        if (Score.score > displayedScore)
        {
            shake += 0.125f;
        }
        displayedScore = Score.score;
        text.text = staticText + " " + displayedScore;
        transform.localPosition = anchorPosition + (Vector3)Random.insideUnitCircle.normalized * shake;
        shake = Mathf.Max(0, shake - Time.deltaTime);
    }
}
