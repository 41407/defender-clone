using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public float levelWidth = 70;

    void Awake()
    {
        cam = Camera.main;
    }
}
