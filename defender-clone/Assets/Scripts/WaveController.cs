using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private GameController gameController;
    public bool waveInProgress = false;
    public GameObject enemySpawnerPrefab;
    public GameObject[] enemyPrefabs;

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
        yield return new WaitForSeconds(2);
        float levelWidth = gameController.levelWidth;
        float step = levelWidth / (Mathf.Max(1, (float)difficulty));
        Vector2 newEnemySpawnerPosition = Vector2.zero;
        for (int i = 0; i < difficulty; i++)
        {
            Factory.create.ByReference(enemySpawnerPrefab, newEnemySpawnerPosition + Vector2.up * Random.Range(0, 4), Quaternion.identity, transform);
            newEnemySpawnerPosition += new Vector2(Random.Range(-step * 0.5f, 2 * step), 0);
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
        print("Active enemies: " + number);
        return number;
    }
}
