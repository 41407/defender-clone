using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrusterParticles : MonoBehaviour
{
    private ParticleSystem particles;
    private PlayerController playerController;
    private int idleTick = 0;
    public int idleEmissionTickRate = 3;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        particles.transform.rotation = Quaternion.FromToRotation(Vector3.back, playerController.direction);
    }

    public void Emit()
    {
        particles.Emit(1);
    }

    public void Idle()
    {
        idleTick++;
        if (idleTick >= idleEmissionTickRate)
        {
            idleTick = 0;
            Emit();
        }
    }
}
