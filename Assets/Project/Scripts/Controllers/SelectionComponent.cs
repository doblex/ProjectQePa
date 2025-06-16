using UnityEngine;


public class SelectionComponent : MonoBehaviour
{
    public delegate void SelectionChangedHandler(bool isSelected); // Delegate for selection change events
    public delegate void UpdateSelectionHandler(); // Delegate for selection update events
    public delegate void ShowVisualizer(bool isShown); // Delegate for visualizer display events

    public event SelectionChangedHandler OnSelectionChanged;
    public event UpdateSelectionHandler OnUpdateSelection;
    public event ShowVisualizer OnShowVisualizer;

    [Header("Selection Settings")]
    [SerializeField] LayerMask layer; // Layer mask to filter raycast hits
    [SerializeField] Transform parent;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SelectionMode selectionMode; // Enum for selection modes
    [SerializeField] bool usesSelectionShader = true;
    [SerializeField, ShowIf("usesSelectionShader", true)] Material selectionShader;

    [Header("KeyBinds Settings")]
    [SerializeField] KeyCode selectionKey = KeyCode.Mouse0;

    [SerializeField,ReadOnly] bool isSelected = false;

    private void Awake()
    {
        if (parent == null)
        {
            Debug.LogError("parent transform is not set");
            return;
        }
        if (meshRenderer != null) return;

        if (!TryGetComponent(out meshRenderer))
        {
            Debug.LogError("MeshRenderer is not assigned and could not be found on the GameObject. Please assign a MeshRenderer component.");
            return;
        }
    }

    private void Update()
    {
        CheckForSelection();
        if (isSelected)
        { 
            OnUpdateSelection?.Invoke();
        }
    }

    private void CheckForSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layer))
        {
            if (hit.transform == parent)
            {
                OnShowVisualizer?.Invoke(true);
                if (Input.GetKeyDown(selectionKey))
                {
                    if (selectionMode == SelectionMode.selection)
                    {
                        if (isSelected)
                        {
                            isSelected = false;
                            OnSelectionChanged?.Invoke(isSelected);

                            if (usesSelectionShader)
                                GameObjectExtension.SetMaterial(meshRenderer, selectionShader);

                            return;
                        }
                    }

                    isSelected = true;
                    OnSelectionChanged?.Invoke(isSelected);

                    if (usesSelectionShader)
                        GameObjectExtension.SetMaterial(meshRenderer, selectionShader);

                }
            }
            else if (!isSelected)
            {
                OnShowVisualizer?.Invoke(false);
            }
        }
        else if (!isSelected)
        {
            OnShowVisualizer?.Invoke(false);
        }

        if (selectionMode == SelectionMode.click)
        {
            if (Input.GetKeyUp(selectionKey) && isSelected)
            {
                isSelected = false;
                OnSelectionChanged?.Invoke(isSelected);

                if (usesSelectionShader)
                    GameObjectExtension.SetMaterial(meshRenderer, selectionShader);
            }
        }
    }
}
