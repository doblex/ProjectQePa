using UnityEngine;

public class RangeVisualizer : Visualizer
{
    [SerializeField] float range = 1f;

    protected override void Draw()
    {
        float angle = 0f;
        lineRenderer.positionCount = resolution + 1;

        for (int i = 0; i <= resolution; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            Vector3 pos = new Vector3(x, 0f, z) + transform.position;
            lineRenderer.SetPosition(i, pos);
            angle += 360f / resolution;
        }
    }
}
