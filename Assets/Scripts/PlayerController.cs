using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _playerInputHandler;
    [SerializeField] private Vector3 movement;
    [SerializeField] private float raycastDistance = 5f;
    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerInputHandler.onInteract.AddListener(DetectInteractableObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        movement = _playerInputHandler._movementInput;
    }


    private void DetectInteractableObject()
    {
        RaycastHit hit; 
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hit, raycastDistance);
        if(hit.collider != null && hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
        {
            Vector3 hitDirection = hit.point - transform.position;
            obj.Interact(hitDirection);
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");
        }
    }

    private void TeleportPlayer()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * raycastDistance);
    }

}
