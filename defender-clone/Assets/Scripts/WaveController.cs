using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private int wave;
    private GameController gameController;
    public bool waveInProgress = false;
    public GameObject enemySpawnerPrefab;
    [System.SerializableAttribute]
    public struct EnemyWaves
    {
        public GameObject enemyPrefab;
        public int appearsAfterWave;
    }
    public EnemyWaves[] enemyPrefabs;
    public GameObject astronautPrefab;

    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void StartNewWave(int wave)
    {
        waveInProgress = true;
        StartCoroutine(StartWave(wave));
    }

    public GameObject GetEnemyPrefab()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        var availableEnemies = from enemy in enemyPrefabs
                               where enemy.appearsAfterWave <= wave
                               select enemy.enemyPrefab;
        return availableEnemies.ElementAt(Random.Range(0, availableEnemies.Count()));
    }

    private IEnumerator StartWave(int wave)
    {
        this.wave = wave;
        yield return new WaitForSeconds(1);
        if (wave % 5 == 0 && GameObject.FindGameObjectsWithTag("Astronaut").Length < 4)
        {
            for (int i = 0; i < 5; i++)
            {
                Factory.create.ByReference(astronautPrefab, new Vector2(Random.Range(0, gameController.levelWidth), 0), Quaternion.identity);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3 + wave * 2; i++)
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
