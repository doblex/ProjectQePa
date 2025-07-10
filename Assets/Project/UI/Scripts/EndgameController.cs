using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using utilities.Controllers;

public class EndgameController : DocController
{
    [Header("Win")]
    [SerializeField] Texture2D winImage;
    [SerializeField] Texture2D winTitleImage;
    [Header("Lose")]
    [SerializeField] Texture2D loseImage;
    [SerializeField] Texture2D loseTitleImage;

    Label collectibles;

    Button retry;
    Button mainMenu;
    Button exit;

    VisualElement backGroundImage;
    VisualElement titleImage;

    int levelCol;

    protected override void SetComponents()
    {
        collectibles = Root.Q<Label>("Collectibles");

        backGroundImage = Root.Q<VisualElement>("Background");
        titleImage = Root.Q<VisualElement>("Title");

        retry = Root.Q<Button>("Retry");
        retry.clicked += OnRetryButton_Clicked;

        mainMenu = Root.Q<Button>("MainMenu");
        mainMenu.clicked += OnMainMenuButton_Clicked;

        exit = Root.Q<Button>("Exit");
        exit.clicked += OnExitButton_Clicked;

        SceneManager.sceneLoaded += GetLevelCollectibles;
    }

    public void GetLevelCollectibles(Scene arg0, LoadSceneMode arg1)
    {
        if(FindAnyObjectByType<ScoreManager>() != null)
        {
            levelCol = ScoreManager.Instance.GetTotalCollectibles;
        }
    }

    public void ShowDoc(bool show, EndgameType endgame)
    { 
        ShowDoc(show);

        if (show)
        {
            switch (endgame)
            {
                case EndgameType.Win:
                    backGroundImage.style.backgroundImage = winImage;
                    titleImage.style.backgroundImage = winTitleImage;
                    retry.style.display = DisplayStyle.None;

                    break;
                case EndgameType.Lose:
                    backGroundImage.style.backgroundImage = loseImage;
                    titleImage.style.backgroundImage = loseTitleImage;
                    retry.style.display = DisplayStyle.Flex;
                    break;
            }

            //Time.timeScale = 0f; // Pause the game
        }

        collectibles.text = $" {ScoreManager.Instance.GetCollected} / {ScoreManager.Instance.GetTotalCollectibles}" ;
    }

    public void SubcribeToHealth(HealthController controller)
    {
        controller.onDeath += OnDeath;
    }

    public void UnsubcribeToHealth(HealthController controller)
    {
        controller.onDeath -= OnDeath;
    }

    private void OnDeath()
    { 
        ShowDoc(true, EndgameType.Lose);
        LevelManager.Instance.ResetLevel();
    }

    private void OnRetryButton_Clicked()
    {
        //Rincomincia da capo (?)
        ShowDoc(false); // Hide the endgame UI
        LevelManager.Instance.RestartLevel();
        Time.timeScale = 1f; // Resume the game
    }

    private void OnMainMenuButton_Clicked()
    {
        Time.timeScale = 1f; // Resume the game
        UIController.Instance.ReturnToMenu();
    }

    private void OnExitButton_Clicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
