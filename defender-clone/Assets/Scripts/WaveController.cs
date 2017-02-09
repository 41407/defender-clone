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
        float levelWidth = gameController.levelWidth;
        float step = levelWidth / 5f;
        Vector2 newSpawnerPosition = Vector2.zero;
        if (difficulty % 5 == 0 && GameObject.FindGameObjectsWithTag("Astronaut").Length < 4)
        {
            for (int i = 0; i < 5; i++)
            {
                Factory.create.ByReference(astronautPrefab, newSpawnerPosition, Quaternion.identity);
                newSpawnerPosition += new Vector2(i * step, 0);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1);
        step = levelWidth / (Mathf.Max(1, (float)difficulty));
        for (int i = 0; i < difficulty; i++)
        {
            Factory.create.ByReference(enemySpawnerPrefab, newSpawnerPosition + Vector2.up * Random.Range(0, 4), Quaternion.identity, transform);
            newSpawnerPosition += new Vector2(Random.Range(-step * 0.5f, 2 * step), 0);
            yield return new WaitForSeconds(Random.Range(0.1f, 1.1f));
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
