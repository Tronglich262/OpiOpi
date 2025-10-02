using UnityEngine;

public class CameraAutoShake : MonoBehaviour
{
    public float amplitude = 0.2f; // độ rung
    public float frequency = 2f;   // tần số rung
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * frequency) * amplitude;
        float offsetY = Mathf.Cos(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(offsetX, offsetY, 0);
    }
}
