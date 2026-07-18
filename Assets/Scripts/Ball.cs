using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallColor Color { get; private set; }

    public void SetColor(BallColor color, Material material)
    {
        Color = color;
        GetComponent<Renderer>().material = material;
    }

    public void MoveTo(Vector3 target)
    {
        transform.position = target;
    }
}
