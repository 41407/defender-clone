using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private GameController gameController;
    public int wave;
    public bool waveInProgress = false;
    public GameObject enemySpawnerPrefab;
    [System.SerializableAttribute]
    public struct EnemyPrefabs
    {
        public GameObject enemyPrefab;
        public int appearsAfterWave;
    }
    public EnemyPrefabs[] enemyPrefabs;
    public GameObject astronautPrefab;
    public int astronautsPerWave;

    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    void Update()
    {
        if (!waveInProgress)
        {
            waveInProgress = true;
            StartCoroutine(WaveCo());
            StartCoroutine(SpawnRandomExtraEnemy());
            wave++;
        }
    }

    public GameObject GetEnemyPrefab()
    {
        var availableEnemies = from enemy in enemyPrefabs
                               where enemy.appearsAfterWave <= wave
                               select enemy.enemyPrefab;
        return availableEnemies.ElementAt(Random.Range(0, availableEnemies.Count()));
    }

    private IEnumerator WaveCo()
    {
        yield return new WaitForSeconds(1);
        if (wave % 5 == 0 && GameObject.FindGameObjectsWithTag("Astronaut").Length < 4)
        {
            yield return SpawnAstronauts();
        }
        yield return new WaitForSeconds(1);
        yield return SpawnEnemies();
        while (GetNumberOfAliveEnemies() > 3)
        {
            yield return new WaitForSeconds(0.5f);
        }
        waveInProgress = false;
    }

    private IEnumerator SpawnAstronauts()
    {
        for (int i = 0; i < astronautsPerWave; i++)
        {
            Vector2 spawnPosition;
            spawnPosition.x = Random.Range(0, gameController.levelWidth);
            spawnPosition.y = 0;
            Factory.create.ByReference(astronautPrefab, spawnPosition, Quaternion.identity);
            yield return null;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int count = 3 + wave * 2;
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnRandomExtraEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8, 12));
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition;
        spawnPosition.x = Random.Range(0, gameController.levelWidth);
        spawnPosition.y = Random.Range(-2, 4);
        Factory.create.ByReference(enemySpawnerPrefab, new Vector2(Random.Range(0, gameController.levelWidth), Random.Range(-2, 4)), Quaternion.identity, transform);
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
