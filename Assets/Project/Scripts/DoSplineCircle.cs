using System;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class DoSplineCircle : MonoBehaviour
{
    [SerializeField] bool ExecuteAction;
    [SerializeField] bool closeSpline;
    [SerializeField] GameObject go;
    [SerializeField] int numPoints = 100;

    private void CreateCircle()
    {
        if (go == null) return;

        SplineContainer splineContainer = go.GetComponent<SplineContainer>();

        if (splineContainer == null) return;

        Spline spline = splineContainer.Spline;
        spline.Closed = closeSpline; // Ensure the spline is closed to form a circle
        spline.Clear();

        float angleStep = 360f / numPoints;
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            BezierKnot splinePoint = new BezierKnot(point);

            spline.Add(splinePoint);

            
        }
    }

    private void OnValidate()
    {
        if (ExecuteAction)
        {
            ExecuteAction = false;
            CreateCircle();
        }
    }
}
