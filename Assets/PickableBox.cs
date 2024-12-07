using UnityEngine;

public class PickableBox : MonoBehaviour, IInteractable
{   
    public bool hasBeenPickedUp = false;
    private Rigidbody _rb;
    private Transform _playerObjHolder;

    private void Start()
    {
        _playerObjHolder = FindFirstObjectByType<PlayerController>().objHolder;
        _rb = GetComponent<Rigidbody>();
    }

    public void Interact(Vector3 hitDirection)
    {
        transform.SetParent(_playerObjHolder);
        transform.position = _playerObjHolder.transform.position;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        hasBeenPickedUp = true;
    }

    public void Release()
    {
        transform.SetParent(null);
        _rb.useGravity = true;
        _rb.isKinematic = false;
        hasBeenPickedUp = false;
    }
}
