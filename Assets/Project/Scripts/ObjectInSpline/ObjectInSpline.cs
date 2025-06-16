using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SelectionComponent))]
public class ObjectInSpline : MonoBehaviour
{
    [Header("Spline Ref")]
    [SerializeField] SplineContainer splineContainer;

    [Header("Visualizer options")]
    [SerializeField] Visualizer visualizer;
    [SerializeField] Material visualizerMaterial;

    [Header("Options")]
    [SerializeField] float movementSpeed = 5f;

    SelectionComponent selectionComponent;

    float currentPosition = 0f;
    float targetPosition = 0f;

    private void Awake()
    {
        selectionComponent = GetComponent<SelectionComponent>();
    }

    private void Start()
    {
        selectionComponent.OnUpdateSelection += OnUpdateSelection;
        selectionComponent.OnShowVisualizer += OnShowVisualizer;
    }

    private void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * movementSpeed);

        splineContainer.Evaluate( 0, currentPosition, out float3 position, out float3 tangent, out float3 upVector);

        transform.position = transform.localToWorldMatrix * (Vector3)position;
    }

    private void OnUpdateSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform splineTransform = splineContainer.transform;

        Vector3 localOrigin = splineTransform.InverseTransformPoint(ray.origin);
        Vector3 localDirection = splineTransform.InverseTransformDirection(ray.direction);
        Ray localRay = new Ray(localOrigin, localDirection);

        SplineUtility.GetNearestPoint(splineContainer[0], localRay, out float3 distance, out targetPosition);
    }

    private void OnShowVisualizer(bool isShown) 
    { 
        visualizer.ShowSpline(isShown, visualizerMaterial);
    }
}
