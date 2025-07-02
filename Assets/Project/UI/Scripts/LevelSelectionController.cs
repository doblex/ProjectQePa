using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectionController : DocController
{
    ScrollView scrollView;
    LevelDataWrapper currentLevel;

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
        scrollView.Clear();

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
                if (levelDataWrapper.comingSoon) return;
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
                levelButton.SetEnabled(false);
            }
            
            scrollView.Add(level);
        }
    }

    private void LoadLevel(LevelDataWrapper levelData)
    {
        UIController.Instance.ShowLoading();
        UIController.Instance.HideLevelSelection();

        if (levelData.level.isCompleted)
        { 
            levelData.level.checkpointIndex = 0; // Reset checkpoint index if level is completed
            levelData.level.isCompleted = false;
            levelData.level.collectibleRecord = 0; // Reset collectible record
            levelData.level.playerLives = 3; // Reset player lives
        }

        currentLevel = levelData;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadScene(levelData.SceneName);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != currentLevel.SceneName) return;

        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        LevelManager.Instance.SetCurrentLevelDataWrapper(currentLevel);
        UIController.Instance.HideLoading();

        if (currentLevel.CutScene != null)
        {
            UIController.Instance.ShowVideo(currentLevel.CutScene, () =>
            {
                UIController.Instance.ShowHUD();
            });
        }
        else
        {
            UIController.Instance.ShowHUD();
        }
    }
}
