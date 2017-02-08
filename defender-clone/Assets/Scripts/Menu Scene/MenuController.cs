using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AsyncOperation sceneLoadOperation;
    private bool menuFinished = false;

    void Start()
    {
        StartCoroutine(EnableChildren());
        sceneLoadOperation = SceneManager.LoadSceneAsync("Game");
        sceneLoadOperation.allowSceneActivation = false;
    }

    void Update()
    {
        if (menuFinished && Input.GetMouseButtonUp(0))
        {
            sceneLoadOperation.allowSceneActivation = true;
            menuFinished = false;
        }
    }

    private IEnumerator EnableChildren()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            if (i < transform.childCount - 2)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                menuFinished = true;
            }
        }
    }
}
