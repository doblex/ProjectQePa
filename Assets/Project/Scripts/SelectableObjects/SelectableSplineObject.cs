using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SelectableSplineObject : SelectableObject
{
    [Header("Spline Reference")]
    [SerializeField] protected SplineContainer splineContainer;

    [Header("Spline Options")]
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected GameObject parent;

    Spline spline;

    float currentPosition = 0f;
    float targetPosition = 0f;

    protected override void Start()
    {
        base.Start();
        spline = splineContainer.Spline;
    }

    protected override void Update()
    {
        base.Update();
        MoveObject();
    }

    private void MoveObject()
    {
        // Smoothly interpolate the current spline position
        if (spline.Closed)
        {
            float delta = Mathf.DeltaAngle(currentPosition * 360f, targetPosition * 360f) / 360f;
            currentPosition += delta * Time.deltaTime * movementSpeed;

            // Wrap currentPosition into 0–1 range
            if (currentPosition < 0f) currentPosition += 1f;
            else if (currentPosition > 1f) currentPosition -= 1f;
        }
        else
        {
            currentPosition = Mathf.MoveTowards(currentPosition, targetPosition, Time.deltaTime * movementSpeed);
        }

        // Evaluate the spline in local space
        spline.Evaluate(currentPosition, out float3 localPos, out float3 localTangent, out float3 localUp);

        // Convert to world space
        Transform splineTransform = splineContainer.transform;
        Vector3 worldPos = splineTransform.TransformPoint((Vector3)localPos);
        Vector3 worldTangent = splineTransform.TransformDirection((Vector3)localTangent);
        Vector3 worldUp = splineTransform.TransformDirection((Vector3)localUp);

        // Set the position and orientation

        if (parent != null)
        {
            parent.transform.position = worldPos;
            parent.transform.rotation = Quaternion.LookRotation(worldTangent, worldUp);
        }
        else
        {
            transform.position = worldPos;
            transform.rotation = Quaternion.LookRotation(worldTangent, worldUp);
        }
    }

    protected override void OnUpdateSelection()
    {
        base.OnUpdateSelection();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform splineTransform = splineContainer.transform;

        // Convert ray to spline local space
        Vector3 localOrigin = splineTransform.InverseTransformPoint(ray.origin);
        Vector3 localDirection = splineTransform.InverseTransformDirection(ray.direction);
        Ray localRay = new Ray(localOrigin, localDirection);

        // Find the nearest position on the spline
        SplineUtility.GetNearestPoint(spline, localRay, out float3 nearestPoint, out targetPosition);

        // Clamp to valid spline range (0–1)
        //targetPosition = math.clamp(targetPosition, 0f, 1f);
    }
}
