using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] bool usesSelectionShader = true;
    [SerializeField, ShowIf("usesSelectionShader", true)] Material selectionShader;

    [Header("KeyBinds Settings")]
    [SerializeField] KeyCode selectionKey = KeyCode.Mouse0;

    MeshRenderer meshRenderer;

    [SerializeField,ReadOnly] bool isSelected = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        CheckForSelection();
        if(isSelected)
            OnUpdateSelection?.Invoke();
    }

    private void CheckForSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                OnShowVisualizer?.Invoke(true);
                if (Input.GetKeyDown(selectionKey))
                {
                    isSelected = true;
                    OnSelectionChanged?.Invoke(isSelected);

                    SetMaterial(selectionShader);
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

        if (Input.GetKeyUp(selectionKey) && isSelected)
        {
            isSelected = false;
            OnSelectionChanged?.Invoke(isSelected);

            SetMaterial(selectionShader);
        }
    }

    private void SetMaterial( Material material)
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
