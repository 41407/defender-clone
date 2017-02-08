using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour
{
    private static Factory instance;

    public static Factory create
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Factory>();
            }
            return instance;
        }
    }

    GameObject InitializeParameters(GameObject created, Transform parent)
    {
        created.transform.parent = parent;
        created.SetActive(true);
        return created;
    }

    public GameObject ByReference(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        return ByReference(gameObject, position, rotation, null);
    }

    public GameObject ByReference(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
    {
        return InitializeParameters(ObjectPool.pool.Pull(gameObject, position, rotation), parent);
    }
}