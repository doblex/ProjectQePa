﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class OptionsController : DocController
{
    [SerializeField] AudioMixer audioMixer;

    Button videoButton;
    VisualElement videoPanel;

    Button audioButton;
    VisualElement audioPanel;

    Button controlsButton;
    VisualElement controlsPanel;

    Button dangerZoneButton;
    VisualElement dangerZonePanel;
    Button deleteSavesButton;

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

        dangerZonePanel = Root.Q<VisualElement>("DangerZonePanel");
        dangerZoneButton = Root.Q<Button>("Saves");
        dangerZoneButton.clicked += DangerZoneButton_clicked;

        deleteSavesButton = Root.Q<Button>("DeleteSaves");
        deleteSavesButton.clicked += () =>
        {
            PersistenceManager.Instance.DeleteSave();
            PersistenceManager.Instance.LoadData();
            Debug.Log("All saves deleted.");
        };

        backButton = Root.Q<Button>("Back");
        backButton.clicked += OnBackButton_Clicked;
    }



    private void SetOptions() 
    {
        Options options = UIController.Instance.Options;

        GetResolution(options, out int width, out int height, out RefreshRate refreshRate);

        Screen.SetResolution(width, height, options.Fullscreen, refreshRate);
        Debug.Log($"Set resolution to {width}x{height} with fullscreen mode {options.Fullscreen} and target FPS {refreshRate}.");

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(options.SFXVolume/ 100 ) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(options.MasterVolume / 100) * 20);
    }

    private void GetResolution(Options options, out int width, out int height, out RefreshRate refreshRate)
    {
        string resolution = options.Resolution[options.SelectedResolutionIndex];

        string[] dimensions = resolution.Split('x');

        width = int.Parse(dimensions[0]);
        height = int.Parse(dimensions[1]);

        // FPS no cap
        if (options.SelectedFPSIndex == 0)
        {
            refreshRate = new RefreshRate() { numerator = 0, denominator = 1 }; // No cap
        }
        else
        { 
            refreshRate = new RefreshRate() { numerator = uint.Parse(options.FPS[options.SelectedFPSIndex]), denominator = 1 };
        }
    }

    public void PopulatePanels() 
    {
        Options options = UIController.Instance.Options;

        controlsPanel.Clear();

        // Populate Commands

        foreach (var cmd in options.commands)
        {
            VisualElement commandVE = new();

            UIController.Instance.CommandTemplate.CloneTree(commandVE);

            commandVE.Q<Label>("Label").text = cmd.CommandName;
            commandVE.Q<VisualElement>("Value").style.backgroundImage = cmd.Icon;

            controlsPanel.Add(commandVE);
        }
    }

    private void UnselectAll()
    { 
        videoButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);
        audioButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);
        controlsButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);
        dangerZoneButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);

        videoPanel.style.display = DisplayStyle.None;
        audioPanel.style.display = DisplayStyle.None;
        controlsPanel.style.display = DisplayStyle.None;
        dangerZonePanel.style.display = DisplayStyle.None;
    }

    private void ShowPanel(Button button, VisualElement activePanel)
    {
        UnselectAll();

        button.AddToClassList(UIController.Instance.ButtonSelectedStyleClass);
        activePanel.style.display = DisplayStyle.Flex;
    }

    private void OnVideoButton_Clicked()
    {
        ShowPanel(videoButton, videoPanel);
    }

    private void OnAudioButton_Clicked()
    {
        ShowPanel(audioButton, audioPanel);
    }

    private void OnControlsButton_Clicked()
    {
        PopulatePanels();
        ShowPanel(controlsButton, controlsPanel);
    }

    private void DangerZoneButton_clicked()
    {
        ShowPanel(dangerZoneButton, dangerZonePanel);
    }

    private void OnBackButton_Clicked()
    {
        UnselectAll();
        SetOptions();
        UIController.Instance.HideOptions();
    }
}
