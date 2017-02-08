using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject spawnParticlePrefab;
    public GameObject enemyPrefab;
    public float spawnDelay;

    void OnEnable()
    {
        StartCoroutine(SpawnAfterDelay(spawnDelay));
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        Instantiate(spawnParticlePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
