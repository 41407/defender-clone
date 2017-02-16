using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    private WaveController waveController;
    public GameObject spawnParticlePrefab;
    public float spawnDelay;
    private float currentTime;

    void Awake()
    {
        waveController = Component.FindObjectOfType<WaveController>();
    }

    void OnEnable()
    {
        currentTime = 0;
        Factory.create.ByReference(spawnParticlePrefab, transform.position, Quaternion.identity);
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime >= spawnDelay)
        {
            Factory.create.ByReference(waveController.GetEnemyPrefab(), transform.position, Quaternion.identity, transform.parent);
            gameObject.SetActive(false);
        }
    }
}
