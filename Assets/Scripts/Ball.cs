using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallColor Color { get; private set; }

    private Coroutine activeMove;

    public void SetColor(BallColor color, Material material)
    {
        Color = color;
        GetComponent<Renderer>().material = material;
    }

    public void MoveTo(Vector3 target)
    {
        if (activeMove != null) StopCoroutine(activeMove);
        transform.position = target;
    }

    public void ArcTo(Vector3 target, float travelHeight)
    {
        if (activeMove != null) StopCoroutine(activeMove);
        if (!gameObject.activeInHierarchy)
        {
            transform.position = target;
            return;
        }
        activeMove = StartCoroutine(Arc(target, travelHeight));
    }

    private IEnumerator Arc(Vector3 target, float travelHeight)
    {
        Vector3 start = transform.position;
        Vector3 up = new Vector3(start.x, travelHeight, start.z);
        Vector3 over = new Vector3(target.x, travelHeight, target.z);

        yield return Segment(start, up, 0.12f);
        yield return Segment(up, over, 0.16f);
        yield return Segment(over, target, 0.12f);

        transform.position = target;
        activeMove = null;
    }

    private IEnumerator Segment(Vector3 a, Vector3 b, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(a, b, t);
            yield return null;
        }
        transform.position = b;
    }
}