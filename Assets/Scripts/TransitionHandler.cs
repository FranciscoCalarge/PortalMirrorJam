using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class TransitionHandler : MonoBehaviour
{
    public static TransitionHandler instance;

    [SerializeField] private Animator _animator;
    public Scene currentScene;
    public int currentSceneIndex;
    public int allScenesAmount;

    private bool _isFading = false;
    private bool _hasFaded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        allScenesAmount = SceneManager.sceneCountInBuildSettings;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        
        if (currentSceneIndex == SceneManager.GetSceneByBuildIndex(0).buildIndex) // Assuming "MainMenu" is at index 0
        {
            MainMenu mainMenu = FindFirstObjectByType<MainMenu>();
            mainMenu.onStartGamePressed.AddListener(LoadNextPuzzle);
        }
    }

    public void FadeIn()
    {
        _isFading = true;
        _animator.SetBool("FadeIn", true);
    }

    public void FadeInCompleted()
    {
        _hasFaded = true;
        _isFading = false;
    }

    public void FadeOut()
    {
        _animator.SetBool("FadeIn", false);
        _animator.SetTrigger("FadeOut");
        _hasFaded = false;
    }

    public void LoadNextPuzzle()
    {
        // Start the fade-out transition
        FadeIn();

        StartCoroutine(LoadSceneAfterFade());
    }

    private IEnumerator LoadSceneAfterFade()
    {
        while (!_hasFaded)
        {
            yield return null;
        }

        currentSceneIndex++;

        if (currentSceneIndex >= allScenesAmount)
        {
            currentSceneIndex = 0;
        }

        SceneManager.LoadScene(currentSceneIndex);

        FadeOut();
    }
}
