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
        Factory.create.ByReference(spawnParticlePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay);
        Factory.create.ByReference(enemyPrefab, transform.position, Quaternion.identity, transform.parent);
        gameObject.SetActive(false);
    }
}
