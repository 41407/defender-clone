using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public float levelWidth = 70;
    public GameObject player;

    void Awake()
    {
        cam = Camera.main;
    }

    void OnEnable()
    {
        player = Component.FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        if (player == null)
        {
            print("Lost a life!");
        }
    }
}
