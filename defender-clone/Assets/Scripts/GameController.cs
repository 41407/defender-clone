using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public float levelWidth = 70;
    public GameObject player;
    public GameObject playerPrefab;
    public int lives = 3;

    void Awake()
    {
        cam = Camera.main;
    }

    void OnEnable()
    {
        StartCoroutine(CoreCo());
    }

    private IEnumerator CoreCo()
    {
        while (lives >= 0)
        {
            yield return new WaitForSeconds(1);
            player = Instantiate(playerPrefab, (Vector2)cam.transform.position, Quaternion.identity);
            while (player.activeInHierarchy)
            {
                yield return null;
            }
            lives--;
        }
        print("Game over!");
    }
}
