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
    public int allSccenesAmount;

    private bool _isFading = false;
    private bool _hasFaded = false;

    private void Awake()
    {
        if(instance == null)
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
        allSccenesAmount = SceneManager.sceneCountInBuildSettings;
        currentScene = SceneManager.GetActiveScene();
    }

    public void FadeIn()
    {
        _isFading = true;
        _animator.SetBool("FadeIn",true);
    }

    public void FadeInCompleted()
    {
        _hasFaded = true;
        _isFading = false;
    }

    public void FadeOut()
    {
        _animator.SetBool("FadeIn",false);
        _animator.SetTrigger("FadeOut");
        _hasFaded = false;
    }

    public void LoadNextPuzzle()
    {
       
    }
}
