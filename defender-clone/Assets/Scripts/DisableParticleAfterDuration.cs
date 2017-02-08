using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticleAfterDuration : MonoBehaviour
{
    private ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        particles.Play();
        StartCoroutine(DisableAfterDuration(particles.main.duration));
    }

    private IEnumerator DisableAfterDuration(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
