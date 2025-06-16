using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Visualizer : MonoBehaviour 
{
    [Header("Spline Visualizer Properties")]
    [Range(2, 100), SerializeField] protected int resolution = 32;
    [Range(0, 1), SerializeField] protected float width = 0.1f;

    protected LineRenderer lineRenderer;

    protected bool isDrawn = false;

    protected virtual void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowSpline(bool show, Material material = null)
    {
        if(material != null)
        {
           GameObjectExtension.SetMaterial(lineRenderer, material);
        }

        if (!isDrawn) Draw();

        lineRenderer.enabled = show;
    }

    protected abstract void Draw();
}
