using UnityEngine;

[RequireComponent(typeof(SelectionComponent))]
public class SelectableObject : MonoBehaviour
{
    [Header("Visualizer options")]
    [SerializeField] protected Visualizer visualizer;
    [SerializeField] protected Material visualizerMaterial;

    protected SelectionComponent selectionComponent;

    protected virtual void Awake()
    {
        selectionComponent = GetComponent<SelectionComponent>();
    }

    protected virtual void Start()
    {
        selectionComponent.OnUpdateSelection += OnUpdateSelection;
        selectionComponent.OnShowVisualizer += OnShowVisualizer;
    }

    protected virtual void Update()
    {}


    protected virtual void OnUpdateSelection() { }
    

    protected void OnShowVisualizer(bool isShown) 
    {
        ShowVisualizer(isShown);
    }

    protected virtual void ShowVisualizer(bool isShown)
    {
        visualizer.ShowRenderer(isShown, visualizerMaterial);
    }
}
