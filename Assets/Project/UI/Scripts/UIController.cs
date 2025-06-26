using UnityEngine;
using UnityEngine.UIElements;
using static OptionsController;

public class UIController : MonoBehaviour
{
    public enum FromDoc
    {
        MainMenu,
        PauseMenu
    }

    public static UIController Instance { get; private set; }
    public Options Options { get => options; }

    public string ButtonSelectedStyleClass { get => buttonSelectedStyleClass; }
    public string ComingSoonStyleClass { get => comingSoonStyleClass; }

    public VisualTreeAsset CommandTemplate { get => commandTemplate; }
    public VisualTreeAsset LevelTemplate { get => levelTemplate; }
    public string GrayedOutButtonStyleClass { get => grayedOutButtonStyleClass; }

    [Header("Docs")]
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] OptionsController option;
    [SerializeField] CreditsController credits;
    [SerializeField] LevelSelectionController levelSelection;

    [Header("DataSources")]
    [SerializeField] Options options;

    [Header("Styles")]
    [SerializeField] string buttonSelectedStyleClass = "OptionButton-Selected";
    [SerializeField] string comingSoonStyleClass = "ComingSoon";
    [SerializeField] string grayedOutButtonStyleClass = "GrayedOut";

    [Header("Templates")]
    [SerializeField] VisualTreeAsset commandTemplate;
    [SerializeField] VisualTreeAsset levelTemplate;

    private FromDoc docToOptions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("Multiple instances of UIController detected. Destroying duplicate instance.");
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        GoToMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.ShowDoc(true);
    }

    public void GoToMainMenu()
    {
        mainMenu.ShowDoc(true);
        option.ShowDoc(false);
        credits.ShowDoc(false);
        levelSelection.ShowDoc(false);
    }

    public void ShowLevelSelection() 
    {
        levelSelection.ShowDoc(true);
    }

    public void ShowOptions(FromDoc fromDoc)
    {
        option.ShowDoc(true);
        docToOptions = fromDoc;
    }

    public void ShowCredits()
    {
        credits.ShowDoc(true);
    }

    public void HideOptions() 
    {
        option.ShowDoc(false);

        switch (docToOptions)
        {
            case FromDoc.MainMenu:
                mainMenu.ShowDoc(true);
                break;
            case FromDoc.PauseMenu:
                // Assuming there's a PauseMenuController to show
                break;
        }
    }
}
