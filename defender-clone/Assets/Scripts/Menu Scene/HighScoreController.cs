using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    private const string HIGHSCORE_PREF = "HighScore";
    private TextMesh text;

    void Awake()
    {
        text = GetComponent<TextMesh>();
    }

    void Start()
    {
        int highScore = PlayerPrefs.GetInt(HIGHSCORE_PREF, 0);
        int currentScore = Score.score;
        if (currentScore == 0 && highScore == 0)
        {
            text.text = "";
        }
        else if (highScore > currentScore)
        {
            text.text = "Your final score was " + currentScore + ".\nHigh score is " + highScore + ".";
        }
        else
        {
            text.text = currentScore + "! New high score!";
            PlayerPrefs.SetInt(HIGHSCORE_PREF, currentScore);
        }
    }
}
