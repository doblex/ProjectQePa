using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : DocController
{
    Button returnToMenu;
    Button options;
    Button resume;

    protected override void SetComponents()
    {
        returnToMenu = Root.Q<Button>("ReturnToMenu");
        returnToMenu.clicked += ReturnToMenu_clicked;

        options = Root.Q<Button>("Options");
        options.clicked += Options_clicked;

        resume = Root.Q<Button>("Resume");
        resume.clicked += Resume_clicked;
    }

    public void ShowDoc(bool show, bool stopTime)
    {
        ShowDoc(show);

        if (stopTime)
        {
            // Pause the game
            Time.timeScale = show ? 0f : 1f;
        }
    }

    private void Resume_clicked()
    {
        UIController.Instance.HidePause(true);
    }

    private void Options_clicked()
    {
        ShowDoc(false, false);
        UIController.Instance.ShowOptions(UIController.FromDoc.PauseMenu);
    }

    private void ReturnToMenu_clicked()
    {
        UIController.Instance.HidePause(true);
        UIController.Instance.ReturnToMenu();
    }
}





