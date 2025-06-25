using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Options;

public class OptionsController : DocController
{
    [SerializeField] VisualTreeAsset commandTemplate;

    Button videoButton;
    VisualElement videoPanel;

    Button audioButton;
    VisualElement audioPanel;

    Button controlsButton;
    VisualElement controlsPanel;

    Button backButton;

    private void Start()
    {
        // Ensure the options panels are hidden initially
        videoPanel.style.display = DisplayStyle.None;
        audioPanel.style.display = DisplayStyle.None;
        controlsPanel.style.display = DisplayStyle.None;
    }

    protected override void SetComponents()
    {
        videoButton = Root.Q<Button>("Video");
        videoButton.clicked += OnVideoButton_Clicked;

        videoPanel = Root.Q<VisualElement>("VideoPanel");

        audioButton = Root.Q<Button>("Audio");
        audioButton.clicked += OnAudioButton_Clicked;

        audioPanel = Root.Q<VisualElement>("AudioPanel");

        controlsButton = Root.Q<Button>("Controls");
        controlsButton.clicked += OnControlsButton_Clicked;

        controlsPanel = Root.Q<VisualElement>("ControlsPanel");

        backButton = Root.Q<Button>("Back");
        backButton.clicked += OnBackButton_Clicked;
    }

    public void PopulatePanels() 
    {
        Options options = UIController.Instance.Options;

        controlsPanel.Clear();

        // Populate Commands

        foreach (var cmd in options.commands)
        {
            VisualElement commandVE = new();
            
            commandTemplate.CloneTree(commandVE);

            SerializedObject serializedCmd = new SerializedObject(cmd); // cmd is a ScriptableObject

            // Bind the visual element to the serialized object
            commandVE.Bind(serializedCmd);

            controlsPanel.Add(commandVE);
        }
    }

    private void OnVideoButton_Clicked()
    {
        videoPanel.style.display = DisplayStyle.Flex;
        audioPanel.style.display = DisplayStyle.None;
        controlsPanel.style.display = DisplayStyle.None;

        // Load video settings
    }

    private void OnAudioButton_Clicked()
    {
        audioPanel.style.display = DisplayStyle.Flex;
        videoPanel.style.display = DisplayStyle.None;
        controlsPanel.style.display = DisplayStyle.None;

        // Load audio settings
    }

    private void OnControlsButton_Clicked()
    {
        controlsPanel.style.display = DisplayStyle.Flex;
        videoPanel.style.display = DisplayStyle.None;
        audioPanel.style.display = DisplayStyle.None;

        // Load controls settings
        PopulatePanels();
    }

    private void OnBackButton_Clicked()
    {
        UIController.Instance.HideOptions();
    }
}
