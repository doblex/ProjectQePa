using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class ObjectInSpline : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] SplineVisualizer visualizer;

    MeshRenderer meshRenderer;

    bool isSelected = false;

    float currentPosition = 0f;
    float targetPosition = 0f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        SetSelected();
        SetTargetPosition();
        MoveObject();
    }

    private void MoveObject()
    {
        currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * ObjectMotionManager.Instance.MovementSpeed);

        splineContainer.Evaluate( 0, currentPosition, out float3 position, out float3 tangent, out float3 upVector);

        transform.position = transform.localToWorldMatrix * (Vector3)position;
    }

    private void SetTargetPosition()
    {
        if(!isSelected) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Transform splineTransform = splineContainer.transform;

            Vector3 localOrigin = splineTransform.InverseTransformPoint(ray.origin);
            Vector3 localDirection = splineTransform.InverseTransformDirection(ray.direction);
            Ray localRay = new Ray(localOrigin, localDirection);

            SplineUtility.GetNearestPoint(splineContainer[0], localRay, out float3 distance,out targetPosition);

        }
    }

    private void SetSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                visualizer.ShowSpline();
                if (Input.GetMouseButtonDown(0))
                {
                    isSelected = true;
                    Material material = ObjectMotionManager.Instance.SelectedMaterial;
                    SetMaterial(ref material);
                }
            }
            else if (!isSelected)
            {
                visualizer.ShowSpline(false);
            }
        }
        else if(!isSelected)
        {
            visualizer.ShowSpline(false);
        }

        if (Input.GetMouseButtonUp(0) && isSelected)
        {
            isSelected = false;
            Material material = ObjectMotionManager.Instance.SelectedMaterial;
            SetMaterial(ref material);
        }
    }

    private void SetMaterial(ref Material material)
    {
        List<Material> newMaterials = meshRenderer.sharedMaterials.ToList();

        if (newMaterials.Contains(material))
        {
            newMaterials.Remove(material);
        }
        else
        {
            newMaterials.Add(material);
        }

        meshRenderer.sharedMaterials = newMaterials.ToArray();
    }
}
