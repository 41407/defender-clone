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
        cam = Camera.main;
        waveController = GetComponent<WaveController>();
    }

    void OnEnable()
    {
        StartCoroutine(CoreCo());
        StartCoroutine(WaveCo());
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
        SceneManager.LoadScene("Menu");
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
