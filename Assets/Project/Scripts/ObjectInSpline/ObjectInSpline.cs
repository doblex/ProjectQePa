using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class ObjectInSpline : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;

    bool isSelected = false;

    float currentPosition = 0f;
    float targetPosition = 0f;

    private void Update()
    {
        SetSelected();
        SetTargetPosition();
        MoveObject();
    }

    private void MoveObject()
    {
        //if (!isSelected) return;

        currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * 5f);

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
