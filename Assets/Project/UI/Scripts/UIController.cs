using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using utilities.Controllers;

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
    public VisualTreeAsset HealthUnitTempleate { get => healthUnitTempleate; }
    public bool IsPaused { get => isPaused; }

    [Header("Docs")]
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] OptionsController option;
    [SerializeField] CreditsController credits;
    [SerializeField] LevelSelectionController levelSelection;
    [SerializeField] LoadingController loading;
    [SerializeField] HUDController hud;
    [SerializeField] PauseController pause;

    [Header("DataSources")]
    [SerializeField] Options options;

    [Header("Styles")]
    [SerializeField] string buttonSelectedStyleClass = "OptionButton-Selected";
    [SerializeField] string comingSoonStyleClass = "ComingSoon";
    [SerializeField] string grayedOutButtonStyleClass = "GrayedOut";

    [Header("Templates")]
    [SerializeField] VisualTreeAsset commandTemplate;
    [SerializeField] VisualTreeAsset levelTemplate;
    [SerializeField] VisualTreeAsset healthUnitTempleate;

    private FromDoc docToOptions;

    [SerializeField] bool isPaused = false;

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
        loading.ShowDoc(false);
        hud.ShowDoc(false);
        pause.ShowDoc(false, false);                   
    }

    public void ShowLevelSelection()
    {
        levelSelection.ShowDoc(true);
    }

    public void HideLevelSelection()
    {
        levelSelection.ShowDoc(false);
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
                pause.ShowDoc(true, false);
                break;
        }
    }

    public void ShowLoading()
    { 
        loading.ShowDoc(true);
    }

    public void HideLoading()
    {
        loading.ShowDoc(false);
    }

    public void ShowLoading(float deactivationDelay)
    { 
        loading.ShowDoc(true);
        StartCoroutine(HideLoadingAfterDelay(deactivationDelay));
    }

    private IEnumerator HideLoadingAfterDelay(float deactivationDelay)
    { 
        yield return new WaitForSeconds(deactivationDelay);
        loading.ShowDoc(false);
    }

    public void ShowHUD()
    {
        hud.ShowDoc(true);
    }
    public void HideHUD()
    {
        hud.ShowDoc(false);
    }

    public void HUDSubHealth(HealthController controller)
    { 
        hud.SubcribeToHealth(controller);
    }

    public void HUDUnsubHealth(HealthController controller)
    {
        hud.UnsubcribeToHealth(controller);
    }

    public void ShowPause(bool stopTime)
    {
        pause.ShowDoc(true, stopTime);
        isPaused = true;
    }

    public void HidePause(bool stopTime)
    {
        pause.ShowDoc(false, stopTime);
        isPaused = false;
    }

    public void ReturnToMenu()
    {
        //AT the click on exit button
        Debug.Log("Ritorna al menù");
        GoToMainMenu();
        SceneManager.LoadScene("MainMenu");
    }
}
