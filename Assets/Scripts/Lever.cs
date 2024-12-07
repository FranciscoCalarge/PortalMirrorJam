using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Lever : MonoBehaviour,IInteractable,IDataPersistence
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool isLeverOn;

    public UnityEvent onLeverOn = new UnityEvent();
    public UnityEvent onLeverOff = new UnityEvent();

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

    }

    public void Interact(Vector3 hitDirection)
    {
        if(isLeverOn)
        {
            isLeverOn = false;
            _animator.SetTrigger("ToggleOff");
            onLeverOff?.Invoke();
        }
        else
        {
            isLeverOn = true;
            _animator.SetTrigger("ToggleOn");
            onLeverOn?.Invoke();
        }
    }

    public void LoadData(GameData data)
    {
       
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
