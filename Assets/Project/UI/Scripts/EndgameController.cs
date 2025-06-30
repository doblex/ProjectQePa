using System;
using UnityEngine;
using UnityEngine.UIElements;
using utilities.Controllers;

public class EndgameController : DocController
{
    [SerializeField] string winText;
    [SerializeField] string loseText;

    Label title;

    Label collectibles;

    Button retry;
    Button mainMenu;
    Button exit;

    protected override void SetComponents()
    {
        title = Root.Q<Label>("Title");

        collectibles = Root.Q<Label>("Collectibles");

        retry = Root.Q<Button>("Retry");
        retry.clicked += OnRetryButton_Clicked;

        mainMenu = Root.Q<Button>("MainMenu");
        mainMenu.clicked += OnMainMenuButton_Clicked;

        exit = Root.Q<Button>("Exit");
        exit.clicked += OnExitButton_Clicked;

    }

    public void ShowDoc(bool show, EndgameType endgame)
    { 
        ShowDoc(show);

        if (show)
        {
            switch (endgame)
            {
                case EndgameType.Win:
                    title.text = winText;
                    retry.style.display = DisplayStyle.None;

                    break;
                case EndgameType.Lose:
                    title.text = loseText;
                    retry.style.display = DisplayStyle.Flex;
                    break;
            }

            Time.timeScale = 0f; // Pause the game
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
