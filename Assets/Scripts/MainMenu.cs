using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _mainMenuUIDoc;

    private VisualElement _mainMenuRoot;
    private VisualElement _creditsPanel;

    private Button _playGameButton;
    private Button _creditsButton;
    private Button _closeCreditsButton;
    private Button _quitButton;

    public UnityEvent onStartGamePressed = new UnityEvent();
    void Start()
    {
        _mainMenuUIDoc = GetComponent<UIDocument>();
        _mainMenuRoot = _mainMenuUIDoc.rootVisualElement;

        _playGameButton = _mainMenuRoot.Q<Button>("PlayGameButton");
        _creditsButton = _mainMenuRoot.Q<Button>("CreditsButton");
        _quitButton = _mainMenuRoot.Q<Button>("QuitButton");

        _creditsPanel = _mainMenuRoot.Q<VisualElement>("CreditsPanel");
        if(_creditsPanel != null) Debug.Log("Found credits panel");_closeCreditsButton = _creditsPanel.Q<Button>("CloseCredits");
        

        _playGameButton.clicked += OnPlayButtonPressed;
        _creditsButton.clicked += OpenCredits;
        _closeCreditsButton.clicked += CloseCredits;
        _quitButton.clicked += Quit;
    }

    private void OnPlayButtonPressed()
    {
        onStartGamePressed?.Invoke();
    }

    private void OpenCredits()
    {
        Debug.Log("Opening credits window");
        if(_creditsPanel != null) _creditsPanel.style.display = DisplayStyle.Flex;
    }

    private void CloseCredits()
    {
        Debug.Log("Closing credits window");
        if(_creditsPanel != null) _creditsPanel.style.display = DisplayStyle.None;
    }

    private void Quit()
    {
        Application.Quit();
        Debug.Log("Closing the game");
    }
}
