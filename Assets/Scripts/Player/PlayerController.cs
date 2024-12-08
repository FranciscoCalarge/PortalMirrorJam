using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _playerInputHandler;
    private Animator _animator;
    private Rigidbody _rb;

    [SerializeField] private CinemachineCamera freeLookCamera;

    [SerializeField] private Vector3 movement;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Canvas interactCanvas;
    public Transform objHolder;
    public GameObject cameraFollowObj;
    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerInputHandler.onInteract.AddListener(DetectInteractableObject);

        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("Idle",true);

        _rb = GetComponent<Rigidbody>();

        interactCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        RaycastHit hit; 
        Physics.Raycast(freeLookCamera.transform.position, freeLookCamera.transform.forward,out hit, raycastDistance);
        if(hit.collider != null && hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
        {
            Debug.Log(hit.collider);
            Vector3 hitDirection = hit.point - transform.position;
            interactCanvas.enabled = true;
        }
        else
        {
            interactCanvas.enabled = false;
        }
    }

    private void Move()
    {
        movement = _playerInputHandler._movementInput;
        Vector3 moveDirection = new Vector3(movement.x, 0, movement.z).normalized;
        _rb.linearVelocity = moveDirection * moveSpeed + new Vector3(0, _rb.linearVelocity.y, 0);
        if(movement.magnitude > 0.01f)
        {
            _animator.SetBool("Moving",true);

        }
        else
        {
            _animator.SetBool("Moving",false);
        }
    }

    public int IsHoldingObject()
    {
        return objHolder.childCount;
    }

    private void DetectInteractableObject()
    {
        if (IsHoldingObject() > 0)
        {
            Debug.Log("Player holding an object.Releasing it...");
            objHolder.GetComponentInChildren<PickableBox>().Release();
            return;
        }
        else{
            
        }
        RaycastHit hit; 
        Physics.Raycast(freeLookCamera.transform.position, freeLookCamera.transform.forward,out hit, raycastDistance);
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
        Gizmos.DrawRay(freeLookCamera.transform.position,Camera.main.transform.forward * raycastDistance);
    }

}
