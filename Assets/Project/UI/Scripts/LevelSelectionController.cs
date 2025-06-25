using UnityEngine;
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
        LoadContent();
    }

    private void LoadContent()
    {
        LevelGroupData groupData = UIController.Instance.LevelGroupData;

        foreach (var levelData in groupData.Levels)
        {
            VisualElement level = new();
            UIController.Instance.LevelTemplate.CloneTree(level);

            Button levelButton = level.Q<Button>("button");

            levelButton.iconImage = levelData.Planet;

            levelButton.clicked += () => { LoadLevel(levelData); };

            levelButton.RegisterCallback<PointerOverEvent>(evt =>
            {
                levelButton.AddToClassList(UIController.Instance.ButtonSelectedStyleClass);
                levelButton.iconImage = levelData.LevelScreen;
            });

            levelButton.RegisterCallback<PointerOutEvent>(evt =>
            {
                levelButton.RemoveFromClassList(UIController.Instance.ButtonSelectedStyleClass);
                levelButton.iconImage = levelData.Planet;
            });

            if (!levelData.IsEnded)
            {
                levelButton.AddToClassList(UIController.Instance.GrayedOutButtonStyleClass);
                levelButton.SetEnabled(false);
            }

            if (levelData.comingSoon)
            {
                levelButton.AddToClassList(UIController.Instance.ComingSoonStyleClass);
            }
            
            scrollView.Add(levelButton);
        }
    }

    private void LoadLevel(LevelData level)
    {
        // Implement level loading logic here
        Debug.Log("Level loaded");
    }
}
