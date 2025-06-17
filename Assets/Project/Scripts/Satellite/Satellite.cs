using UnityEngine;

[RequireComponent(typeof(SatelliteVisualizer))]
public class Satellite : MonoBehaviour
{
    [SerializeField] SatelliteDirection direction = SatelliteDirection.clockwise; // Direction of rotation
    [SerializeField] Transform target;     // The object to orbit around
    [SerializeField] float radius = 5f;    // Distance from the target
    [SerializeField] float speed = 50f;    // Degrees per second

    private Vector3 offset;

    public float Radius { get => radius; }

    float DirectionMultiplier
    {
        get
        {
            return direction == SatelliteDirection.clockwise ? 1f : -1f;
        }
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned!");
            return;
        }

        // Set the initial offset from the target
        offset = (target.position - transform.position).normalized * radius;
        target.position = transform.position + offset;
    }

    void Update()
    {
        if (target == null) return;

        // Rotate around the target on the Y axis
        target.RotateAround(transform.position, Vector3.up, DirectionMultiplier * speed * Time.deltaTime);
    }
}
