using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private GameController gameController;
    public bool waveInProgress = false;
    public GameObject enemySpawnerPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject astronautPrefab;

    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void StartNewWave(int difficulty)
    {
        waveInProgress = true;
        StartCoroutine(StartWave(difficulty));
    }

    public GameObject GetEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    private IEnumerator StartWave(int difficulty)
    {
        yield return new WaitForSeconds(1);
        if (difficulty % 5 == 0 && GameObject.FindGameObjectsWithTag("Astronaut").Length < 4)
        {
            for (int i = 0; i < 5; i++)
            {
                Factory.create.ByReference(astronautPrefab, new Vector2(Random.Range(0, gameController.levelWidth), 0), Quaternion.identity);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < difficulty; i++)
        {
            Factory.create.ByReference(enemySpawnerPrefab, new Vector2(Random.Range(0, gameController.levelWidth), Random.Range(-2, 4)), Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
        }

        while (GetNumberOfAliveEnemies() > 2)
        {
            yield return new WaitForSeconds(0.5f);
        }
        waveInProgress = false;
    }

    private int GetNumberOfAliveEnemies()
    {
        int number = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                number++;
            }
        }
        return number;
    }
}
