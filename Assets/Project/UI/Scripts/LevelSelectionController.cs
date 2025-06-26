using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectionController : DocController
{
    ScrollView scrollView;
    

    protected override void SetComponents()
    {
        scrollView = Root.Q<ScrollView>("ScrollView");
    }

    public override void ShowDoc(bool show)
    {
        base.ShowDoc(show);
        if (show)
        {
            LoadContent();
        }
    }

    private void LoadContent()
    {
        LevelDataWrapper[] levelDataWrappers = PersistenceManager.Instance.LevelDataWrappers.ToArray();

        foreach (var levelDataWrapper in levelDataWrappers)
        {
            VisualElement level = new();
            UIController.Instance.LevelTemplate.CloneTree(level);

            Button levelButton = level.Q<Button>("Button");

            levelButton.iconImage = levelDataWrapper.Planet;

            levelButton.clicked += () => { LoadLevel(levelDataWrapper); };

            levelButton.RegisterCallback<PointerOverEvent>(evt =>
            {
                levelButton.AddToClassList(UIController.Instance.ButtonSelectedStyleClass);
                levelButton.iconImage = levelDataWrapper.LevelScreen;
            });

            levelButton.RegisterCallback<PointerOutEvent>(evt =>
            {
                levelButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);
                levelButton.iconImage = levelDataWrapper.Planet;
            });

            if (levelDataWrapper.IsLocked())
            {
                levelButton.AddToClassList(UIController.Instance.GrayedOutButtonStyleClass);
                levelButton.SetEnabled(false);
            }

            if (levelDataWrapper.comingSoon)
            {
                levelButton.AddToClassList(UIController.Instance.ComingSoonStyleClass);
            }
            
            scrollView.Add(level);
        }
    }

    private void LoadLevel(LevelDataWrapper levelData)
    {
        SceneManager.LoadScene(levelData.SceneName);
    }
}
