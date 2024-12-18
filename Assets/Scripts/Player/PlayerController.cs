using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    public Transform objHolder;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private float lookSensitivity = 2f; 
    [SerializeField] private Vector3 movement;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Canvas interactCanvas;

    private PlayerInputHandler _playerInputHandler;
    private Animator _animator;
    private Rigidbody _rb;

    private float _xRotation = 0f;

    [SerializeField] private bool _isGrounded;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerInputHandler.onInteract.AddListener(DetectInteractableObject);

        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("Idle", true);

        _rb = GetComponent<Rigidbody>();

        interactCanvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        HandleCameraRotation();

        if(_playerInputHandler.didJump && _isGrounded)
        {
            Jump();
        }

        Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
            {
                Debug.Log(hit.collider);
                interactCanvas.enabled = true;
            }
        }
        else
        {
            interactCanvas.enabled = false;
        }
    }

    private void Move()
    {
        movement = _playerInputHandler._movementInput;
        Vector3 moveDirection = transform.right * movement.x + transform.forward * movement.z;

        Vector3 velocity = new Vector3(moveDirection.x * moveSpeed, _rb.linearVelocity.y, moveDirection.z * moveSpeed);
        _rb.linearVelocity = velocity;

        if (movement.magnitude > 0.01f)
        {
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _isGrounded = false;
    }

    private void HandleCameraRotation()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        
        if (mouseDelta.magnitude > Mathf.Epsilon) 
        {
            float mouseX = mouseDelta.x * lookSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * lookSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 50f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
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
            Debug.Log("Player holding an object. Releasing it...");
            objHolder.GetComponentInChildren<PickableBox>().Release();
            return;
        }

        Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out IInteractable obj))
            {
                Vector3 hitDirection = hit.point - transform.position;
                obj.Interact(hitDirection);
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        if (firstPersonCamera != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(firstPersonCamera.transform.position, firstPersonCamera.transform.forward * raycastDistance);
        }
    }
}
