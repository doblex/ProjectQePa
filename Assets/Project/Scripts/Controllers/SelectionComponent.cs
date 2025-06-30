using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UIElements;


public class SelectionComponent : MonoBehaviourWithAudio
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
        if (UIController.Instance.IsPaused) return;

        CheckForSelection();
        if (isSelected)
        { 
            OnUpdateSelection?.Invoke();
        }
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        screenPosition.y = Screen.height - screenPosition.y;

        var panel = UIController.Instance.Hud.Root.panel;
        if (panel == null) return false;

        List<VisualElement> picked = new List<VisualElement>();

        panel.PickAll(screenPosition, picked);

        foreach (var element in picked)
        {
            if (element.visible && element.pickingMode == PickingMode.Position &&
                    element.resolvedStyle.display != DisplayStyle.None)
            {
                return true;
            }
        }

        return false;
    }

    private void CheckForSelection()
    {
        if (IsPointerOverUI(Input.mousePosition)) 
        {
            return;
        }

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
                            OnUnselectAudio?.Invoke(audioChannels);

                            if (usesSelectionShader)
                                GameObjectExtension.UnsetMaterial(meshRenderer, selectionShader);

                            return;
                        }
                    }

                    isSelected = true;
                    OnSelectionChanged?.Invoke(isSelected);
                    OnPlayAudio?.Invoke(audioChannels);

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
                OnStopAudio?.Invoke(audioChannels);

                if (usesSelectionShader)
                {
                    GameObjectExtension.UnsetMaterial(meshRenderer, selectionShader);
                }
            }
                   
        }
    }

    public void CallSelectionDisable()
    {
        isSelected = false;
        OnSelectionChanged?.Invoke(isSelected);
        OnStopAudio(audioChannels);

        if (usesSelectionShader)
        {
            GameObjectExtension.UnsetMaterial(meshRenderer, selectionShader);
        }
    }
}
