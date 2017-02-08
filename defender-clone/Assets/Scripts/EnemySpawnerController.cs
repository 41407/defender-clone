using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    private WaveController waveController;
    public GameObject spawnParticlePrefab;
    public float spawnDelay;

    void Awake()
    {
        waveController = Component.FindObjectOfType<WaveController>();
    }

    void OnEnable()
    {
        StartCoroutine(SpawnAfterDelay(spawnDelay));
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        Factory.create.ByReference(spawnParticlePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay);
        Factory.create.ByReference(waveController.GetEnemyPrefab(), transform.position, Quaternion.identity, transform.parent);
        gameObject.SetActive(false);
    }
}
