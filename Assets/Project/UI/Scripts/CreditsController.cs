using UnityEngine.UIElements;

public class CreditsController : DocController
{
    Button backButton;
    protected override void SetComponents()
    {
        backButton = Root.Q<Button>("Back");
        backButton.clicked += OnBackButton_Clicked;
    }
    private void OnBackButton_Clicked()
    {
        ShowDoc(false);
        UIController.Instance.ShowMainMenu();
    }
}
