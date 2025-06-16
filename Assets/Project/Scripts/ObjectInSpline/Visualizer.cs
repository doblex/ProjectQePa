using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Visualizer : MonoBehaviour 
{
    protected LineRenderer lineRenderer;

    protected bool isDrawn = false;

    protected virtual void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowSpline(bool show = true)
    {
        if (!isDrawn) Draw();

        lineRenderer.enabled = show;
    }

    protected abstract void Draw();
}
