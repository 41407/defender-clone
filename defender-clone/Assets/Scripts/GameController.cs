using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public float levelWidth = 70;
    public GameObject player;
    public GameObject playerPrefab;
    public int lives = 3;
    private WaveController waveController;
    public int difficulty = 5;
    public int difficultyIncrementPerWave = 2;

    void Awake()
    {
        Score.score = 0;
        cam = Camera.main;
        waveController = GetComponent<WaveController>();
    }

    void OnEnable()
    {
        StartCoroutine(CoreCo());
        StartCoroutine(WaveCo());
    }

    public void AstronautGotAbducted()
    {
        int remainingAstronauts = GameObject.FindGameObjectsWithTag("Astronaut").Length;
        print("Astronauts remaining: " + remainingAstronauts);
        if (remainingAstronauts <= 0)
        {
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        StartCoroutine(GameOverCo());
    }

    private IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator CoreCo()
    {
        while (lives >= 0)
        {
            yield return new WaitForSeconds(1);
            player = Instantiate(playerPrefab, (Vector2)cam.transform.position, Quaternion.identity);
            while (player != null)
            {
                yield return null;
            }
            lives--;
        }
        OnGameOver();
    }

    private IEnumerator WaveCo()
    {
        while (true)
        {
            if (waveController.waveInProgress)
            {
                yield return null;
            }
            else
            {
                waveController.StartNewWave(difficulty);
                difficulty += difficultyIncrementPerWave;
                yield return null;
            }
        }
    }
}
