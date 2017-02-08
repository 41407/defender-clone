using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public bool waveInProgress = false;
    public GameObject enemySpawnerPrefab;
    private GameController gameController;

    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void StartNewWave(int difficulty)
    {
        waveInProgress = true;
        StartCoroutine(SpawnRegularEnemies(difficulty));
    }

    private IEnumerator SpawnRegularEnemies(int difficulty)
    {
        yield return new WaitForSeconds(2);
        float levelWidth = gameController.levelWidth;
        float step = levelWidth / (Mathf.Max(1, (float)difficulty));
        Vector2 newEnemySpawnerPosition = Vector2.zero;
        for (float enemyX = 0; enemyX < levelWidth; enemyX += step)
        {
            Instantiate(enemySpawnerPrefab, newEnemySpawnerPosition + Vector2.up * Random.Range(0, 4), Quaternion.identity);
            newEnemySpawnerPosition += new Vector2(step, 0);
            yield return new WaitForSeconds(Random.Range(0.1f, 1.1f));
        }
    }
}
