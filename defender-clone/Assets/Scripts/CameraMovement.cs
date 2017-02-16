using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;

    void Update()
    {
        if (player == null)
        {
            try
            {
                player = Component.FindObjectOfType<PlayerController>().transform;
            }
            catch (System.NullReferenceException)
            {
                // wait
            }
        }
        else
        {
            transform.position = player.transform.position;
            transform.position = new Vector3(transform.position.x, 0, -10);
        }
    }
}