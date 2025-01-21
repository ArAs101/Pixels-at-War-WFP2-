using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 9.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80f;

    private Camera playerCamera;
    private CharacterController characterController;
    private bool cameraInitialized = false;

    private Vector3 moveDirection;
    private float rotationX = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;        
        CanMove = true;
    }

    void Update()
    {
        if (!cameraInitialized)
        {
            playerCamera = Camera.main;
            if (playerCamera != null)
            {
                cameraInitialized = true;
                Debug.Log("Main camera ist in playercontroller-klasse verfügbar und initialisiert");
            }
            else
            {
                Debug.LogWarning("Main camera ist in playercontroller-klasse noch nicht verfügbar");
                return;
            }
        }

        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            ApplyFinalMovements();
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }


    public void InitializeCamera()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogWarning("main camera konnte in playercontroller nicht gefunden werden");
        }
        else
        {
            Debug.Log("Main camera ist in playercontroller-klasse verfügbar und initialisiert");
        }
    }

    private void HandleMovementInput()
    {
        Vector2 currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        if (playerCamera == null)
        {
            InitializeCamera();
            if (playerCamera == null)
            {
                return;
            }
        }

        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void EnableMovement()
    {
        CanMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void DisableMovement()
    {
        Debug.Log("Bewegung deaktiviert");
        CanMove = false;
    }

    public void DisableMovementAndShowCursor()
    {
        Debug.Log("Bewegung deaktiviert");
        CanMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}