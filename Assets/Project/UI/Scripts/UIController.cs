using UnityEngine;
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

    [Header("Docs")]
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] OptionsController option;

    [Header("Refs")]
    [SerializeField] Options options;

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
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.ShowDoc(true);
        option.ShowDoc(false);
    }

    public void ShowOptions(FromDoc fromDoc)
    {
        option.ShowDoc(true);
        docToOptions = fromDoc;
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
