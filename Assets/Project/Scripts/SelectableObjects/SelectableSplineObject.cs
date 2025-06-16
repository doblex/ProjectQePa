using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SelectableSplineObject : SelectableObject 
{
    [Header("Spline Ref")]
    [SerializeField] protected SplineContainer splineContainer;

    [Header("Spline Options")]
    [SerializeField] protected float movementSpeed = 5f;

    float currentPosition = 0f;
    float targetPosition = 0f;

    protected override void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * movementSpeed);

        splineContainer.Evaluate(0, currentPosition, out float3 position, out float3 tangent, out float3 upVector);

        transform.position = transform.localToWorldMatrix * (Vector3)position;
    }


    protected override void OnUpdateSelection()
    {
        base.OnUpdateSelection();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform splineTransform = splineContainer.transform;

        Vector3 localOrigin = splineTransform.InverseTransformPoint(ray.origin);
        Vector3 localDirection = splineTransform.InverseTransformDirection(ray.direction);
        Ray localRay = new Ray(localOrigin, localDirection);

        SplineUtility.GetNearestPoint(splineContainer[0], localRay, out float3 distance, out targetPosition);
    }
}