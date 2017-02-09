using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautController : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody2D body;
    public GameObject abductor;
    public bool beingAbducted = false;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        body.isKinematic = false;
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
            if (transform.position.y > 5)
            {
                gameController.AstronautGotAbducted();
                abductor.SetActive(false);
                gameObject.SetActive(false);
            }
            yield return null;
        }
        abductor = null;
        beingAbducted = false;
        body.isKinematic = false;
        body.velocity = Vector2.zero;
    }
}
