using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public float levelWidth = 70;
    public GameObject player;
    public GameObject playerSpawnParticlePrefab;
    public GameObject playerPrefab;
    public int lives = 3;

    void Awake()
    {
        Score.score = 0;
        cam = Camera.main;
    }

    void OnEnable()
    {
        StartCoroutine(CoreCo());
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
            Factory.create.ByReference(playerSpawnParticlePrefab, (Vector2)cam.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2);
            player = Instantiate(playerPrefab, (Vector2)cam.transform.position, Quaternion.identity);
            while (player != null)
            {
                yield return null;
            }
            lives--;
            yield return new WaitForSeconds(2);
        }
        OnGameOver();
    }
}
