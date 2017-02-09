using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMessageViewerController : MonoBehaviour
{
    public Transform awayPosition;
    public Vector3 displayPosition = Vector3.zero;
    public float messageDuration = 2;
    private TextMesh text;
    private string message;
    public string Message
    {
        get
        {
            return message;
        }
        set
        {
            message = value;
            StopAllCoroutines();
            StartCoroutine(DisplayMessage());
        }
    }

    void Awake()
    {
        text = GetComponent<TextMesh>();
    }

    private IEnumerator DisplayMessage()
    {
        yield return LerpTowardsPosition(awayPosition.localPosition);
        text.text = message;
        yield return LerpTowardsPosition(displayPosition);
        yield return new WaitForSeconds(messageDuration);
        yield return LerpTowardsPosition(awayPosition.localPosition);
    }

    private IEnumerator LerpTowardsPosition(Vector3 position)
    {
        while (Vector2.Distance(transform.localPosition, position) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, 5 * Time.deltaTime);
            yield return null;
        }
    }
}
