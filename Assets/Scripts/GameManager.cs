using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public FileDataHandler _fileDataHandler;
    public List<IDataPersistence> dataPersistencesObjs;
    private GameData _gameData;


    private MainMenu _mainMenu;
    private PauseMenu _pauseMenu;
    private TransitionHandler _transitionHandlerInstance;

    private bool _isGamePaused = false;
    public UnityEvent<bool> onGamePaused = new UnityEvent<bool>();

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
    void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        _transitionHandlerInstance = TransitionHandler.instance;

        // _pauseMenu = GetComponent<PauseMenu>();
        // _pauseMenu.onReturnToMenu.AddListener(UnpauseGame);
    }

    // Update is called once per frame
    private void Update()
    {
        if(_transitionHandlerInstance.currentScene.name == "MainMenu")
        {
            _mainMenu = FindFirstObjectByType<MainMenu>();
            _mainMenu.onStartGamePressed.AddListener(StartGame);
        }

        if(!_isGamePaused)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame(true);
            }   
        }
        else if(_isGamePaused)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame(false);
            }
        }
    }

    private void StartGame()
    {
        if (_fileDataHandler == null)
        {
            Debug.LogError("FileDataHandler is not initialized!");
            return;
        }

        _gameData = _fileDataHandler.LoadWithDataName("gameData"); // Try loading saved data

        if (_gameData == null)
        {
            _gameData = new GameData();
        }     
        _transitionHandlerInstance.LoadNextPuzzle();   
    }

    private void PauseGame(bool isPaused)
    {
        if(isPaused)
        {
            Debug.Log("Pausing the game");
            onGamePaused?.Invoke(true);
            _isGamePaused = true;
        }
    }

    private void UnpauseGame()
    {
        _isGamePaused = false;
        onGamePaused?.Invoke(false);
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjs)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }

        _fileDataHandler.SaveWithDataName(_gameData,"gameData");
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    public List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        Debug.Log("Findinf the data Persistences");
        IEnumerable<IDataPersistence> dataPersistencesObjs = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        Debug.Log($"Found {dataPersistencesObjs.Count()} data persistence objects.");
        return new List<IDataPersistence>(dataPersistencesObjs);
    }
}
