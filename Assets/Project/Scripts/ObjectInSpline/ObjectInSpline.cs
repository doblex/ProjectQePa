using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class ObjectInSpline : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;

    bool isSelected = false;

    private void Update()
    {
        SetSelected();
        MoveWithMouse();
    }

    private void MoveWithMouse()
    {
        if(!isSelected) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                float3 hitPoint = hit.point;

                SplineUtility.GetNearestPoint(splineContainer[0], hitPoint, out float3 distance, out float t);

                Vector3 positionOnSpline = SplineUtility.EvaluatePosition(splineContainer[0], t);

                Vector3 worldPositionOnSpline = transform.TransformPoint(positionOnSpline);

                Debug.DrawLine(hitPoint, positionOnSpline, Color.red);

                transform.position = positionOnSpline;
            }
        }
    }

    private void SetSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    isSelected = !isSelected;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSelected = false;
        }
    }
}
