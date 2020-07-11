using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    Inventory _inventory;

    [SerializeField]
    [Range(1, 10)]
    float movementSpeed;
    
    [SerializeField]
    [Range(1, 5)]
    float jumpHeight;

    [SerializeField]
    [Range(0.1f, 10.0f)]
    float mouseSensetivity;

    [SerializeField]
    [Range(0f, 5f)]
    float maxInteractDistance;

    float _yVelocity = 0;

    float _xAngle = 0;
    float _yAngle = 0;

    Camera _mainCam;
    public Plane[] camPlanes;

    public Action<GameObject> click = delegate { };
    public UnityEngine.UI.RawImage openHandImage;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inventory = GetComponent<Inventory>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCam = Camera.main;
        camPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCam);
    }

    void Update()
    {
        HandleMovement();
        HandleInteraction();
        _inventory.HandleInventory();
        camPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCam);
    }

    void HandleMovement()
    {
        Vector3 moveHorizontal = Input.GetAxis("Horizontal") * transform.right * movementSpeed;
        Vector3 moveVertical = Input.GetAxis("Vertical") * transform.forward * movementSpeed;

        if (_controller.isGrounded)
        {
            if (Input.GetAxis("Jump") > 0)
                _yVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            else
                _yVelocity = 0;
        }
        else
        {
            _yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 move = moveHorizontal + moveVertical + new Vector3(0, _yVelocity, 0);
        _controller.Move(move * Time.deltaTime);

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _xAngle += Input.GetAxis("Mouse X") * mouseSensetivity;
            _yAngle += Input.GetAxis("Mouse Y") * mouseSensetivity;
            _yAngle = Mathf.Clamp(_yAngle, -90, 90);

            transform.localRotation = Quaternion.Euler(0, _xAngle, 0);
            _mainCam.transform.localRotation = Quaternion.Euler(-_yAngle, 0, 0);
        }
    }

    void HandleInteraction()
    {
        openHandImage.gameObject.SetActive(false);

        RaycastHit raycast;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out raycast))
        {
            if (raycast.distance <= maxInteractDistance)
            {
                GameObject hit = raycast.collider.gameObject;
                Interactable i = hit.GetComponent<Interactable>();
                if (i != null && i.canInteract)
                {
                    openHandImage.gameObject.SetActive(true);
                    if (Input.GetMouseButtonDown(0) || Input.GetAxis("Interact") > 0)
                        i.Interact(gameObject);
                }
            }
        }
    }
}
