using System;
using UnityEngine;

public class CameraBorderCheck : MonoBehaviour
{
    Camera cam;
    [SerializeField] float offset = 0.1f;

    [SerializeField] Renderer objectRenderer;

    [SerializeField] bool active = false;

    public bool Active { get => active; set => active = value; }

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }

        Invoke(nameof(SetActive), 1f); // Delay to ensure camera is set up
    }

    private void SetActive()
    {
        active = true;
    }

    void Update()
    {
        if (!active) return;

        if (!IsVisibleFrom(cam, offset))
        {
            ResetPlayer();
            active = false; // Disable the check after resetting the player
        }
    }

    private void ResetPlayer()
    {
        LevelManager.Instance.ResetToCheckPoint(false);
    }

    bool IsVisibleFrom(Camera cam, float buffer = 0.1f)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        // Expand planes outward by buffer amount
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].distance -= buffer;
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
            return GeometryUtility.TestPlanesAABB(planes, col.bounds);

        Renderer rend = GetComponent<Renderer>();
        return rend != null && rend.isVisible; // Fallback for objects with a Renderer
    }
}
