using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautController : MonoBehaviour
{
    private Rigidbody2D body;
    public GameObject abductor;
    public bool beingAbducted = false;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        beingAbducted = false;
        abductor = null;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            abductor = coll.gameObject;
            StartCoroutine(GetAbductedCo());
        }
    }

    private IEnumerator GetAbductedCo()
    {
        body.isKinematic = true;
        while (abductor.activeInHierarchy)
        {
            transform.position = Vector3.Lerp(transform.position, abductor.transform.position, 0.1f);
            yield return null;
        }
        abductor = null;
        beingAbducted = false;
        body.isKinematic = false;
        body.velocity = Vector2.zero;
    }
}
