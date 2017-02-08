using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public int defaultPoolSize = 10;
    private static ObjectPool instance;
    public static ObjectPool pool
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ObjectPool>();
            }
            return instance;
        }
    }
    Dictionary<GameObject, List<GameObject>> objectPool = new Dictionary<GameObject, List<GameObject>>();

    public void Initialize(GameObject objectType)
    {
        Initialize(objectType, defaultPoolSize);
    }

    public void Initialize(GameObject objectType, int poolSize)
    {
        if (!objectPool.ContainsKey(objectType))
        {
            InitializeByKey(objectType, poolSize);
        }
    }

    public GameObject Pull(GameObject objectType, Vector3 position, Quaternion rotation)
    {
        if (!objectPool.ContainsKey(objectType))
        {
            Initialize(objectType);
        }
        GameObject newObject = PullFromList(objectPool[objectType], objectType);
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        return newObject;
    }

    void InitializeByKey(GameObject key, int poolSize)
    {
        objectPool.Add(key, new List<GameObject>());
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(key, new Vector3(100, 0, -100), Quaternion.identity);
            newObject.SetActive(false);
            objectPool[key].Add(newObject);
        }
    }

    GameObject PullFromList(List<GameObject> pool, GameObject objectType)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        GameObject newObject = Instantiate(objectType);
        newObject.SetActive(false);
        pool.Add(newObject);
        return newObject;
    }
}
