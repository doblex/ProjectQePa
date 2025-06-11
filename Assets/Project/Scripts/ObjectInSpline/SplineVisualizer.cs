using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer), typeof(LineRenderer))]
public class SplineVisualizer : MonoBehaviour
{
    SplineContainer splineContainer;
    LineRenderer lineRenderer;

    bool isShown = true;

    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        DrawSpline();
        ShowSpline(false);
    }

    public void ShowSpline(bool show = true) 
    {
        lineRenderer.enabled = show;
    }

    public void DrawSpline()
    {
        if (splineContainer == null || lineRenderer == null || splineContainer.Spline.Count < 2)
            return;

        // Sample points along the spline
        int resolution = ObjectMotionManager.Instance.Resolution;
        float width = ObjectMotionManager.Instance.Width;

        lineRenderer.positionCount = resolution + 1;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 localPoint = splineContainer.Spline.EvaluatePosition(t);
            Vector3 worldPoint = splineContainer.transform.TransformPoint(localPoint);

            lineRenderer.SetPosition(i, worldPoint);
        }

        lineRenderer.startWidth = width;
    }
}
