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

        transform.position = SplineUtility.EvaluatePosition(splineContainer[0], currentPosition);
    }

    private void SetTargetPosition()
    {
        if(!isSelected) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            SplineUtility.GetNearestPoint(splineContainer[0], ray, out float3 distance,out targetPosition);
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
                    isSelected = true;
                    visualizer.ShowSpline();
                    Material material = ObjectMotionManager.Instance.SelectedMaterial;
                    SetMaterial(ref material);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSelected = false;
            visualizer.ShowSpline();
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
