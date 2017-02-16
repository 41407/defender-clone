using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private HUDMessageViewerController messageViewer;
    public Camera cam;
    public float levelWidth = 70;
    public float levelHeight = 10;
    public GameObject player;
    public GameObject playerSpawnParticlePrefab;
    public GameObject playerPrefab;
    public int lives = 3;

    void Awake()
    {
        Score.score = 0;
        cam = Camera.main;
        messageViewer = Component.FindObjectOfType<HUDMessageViewerController>();
    }

    void OnEnable()
    {
        StartCoroutine(CoreCo());
    }

    public void AstronautGotAbducted()
    {
        int remainingAstronauts = GameObject.FindGameObjectsWithTag("Astronaut").Length - 1;
        string plural = remainingAstronauts == 1 ? "" : "s";
        if (remainingAstronauts <= 0)
        {
            messageViewer.Message = "All the astronauts have been abducted!";
            OnGameOver();
        }
        else
        {
            messageViewer.Message = remainingAstronauts + " astronaut" + plural + " remaining.";
        }
    }

    private void OnGameOver()
    {
        StartCoroutine(GameOverCo());
    }

    private IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(4);
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
