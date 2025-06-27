using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : DocController
{
    Button playButton;
    Button optionsButton;
    Button creditsButton;
    Button exitButton;

    protected override void SetComponents()
    {
        playButton = Root.Q<Button>("Play");
        playButton.clicked += OnPlayButton_Clicked;

        optionsButton = Root.Q<Button>("Options");
        optionsButton.clicked += OnOptionsButton_Clicked;

        creditsButton = Root.Q<Button>("Credits");
        creditsButton.clicked += OnCreditsButton_Clicked;

        exitButton = Root.Q<Button>("Exit");
        exitButton.clicked += OnExitButton_Clicked;
    }

    private void OnPlayButton_Clicked()
    {
        ShowDoc(false);
        UIController.Instance.ShowLevelSelection();
    }
    private void OnOptionsButton_Clicked()
    {
        ShowDoc(false);
        UIController.Instance.ShowOptions(UIController.FromDoc.MainMenu);
    }
    private void OnCreditsButton_Clicked()
    {
        ShowDoc(false);
        UIController.Instance.ShowCredits();
    }
    private void OnExitButton_Clicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
