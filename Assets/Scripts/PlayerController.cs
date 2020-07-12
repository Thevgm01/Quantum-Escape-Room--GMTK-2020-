using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    [HideInInspector]
    public Inventory _inventory;

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
    bool canMove = true;
    bool canLook = true;

    Camera _mainCam;
    public Plane[] camPlanes;

    public Action<GameObject> click = delegate { };
    public UnityEngine.UI.RawImage openHandImage;
    [HideInInspector]
    public Texture openHand;
    public UnityEngine.UI.Text hoverText;

    AudioSource footsteps;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inventory = GetComponent<Inventory>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _xAngle = transform.rotation.eulerAngles.y;

        _mainCam = Camera.main;
        camPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCam);

        openHand = openHandImage.texture;

        footsteps = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canMove || canLook)
        {
            HandleMovement();
            HandleInteraction();
        }
        _inventory.HandleInventory();
        camPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCam);
    }

    void HandleMovement()
    {
        if (canMove)
        {
            Vector3 moveHorizontal = Input.GetAxis("Horizontal") * transform.right;
            Vector3 moveVertical = Input.GetAxis("Vertical") * transform.forward;

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

            Vector3 lateralMove = moveHorizontal + moveVertical;
            if (lateralMove.magnitude > 1) lateralMove.Normalize();
            Vector3 move = lateralMove * movementSpeed + new Vector3(0, _yVelocity, 0);
            _controller.Move(move * Time.deltaTime);

            if (lateralMove != Vector3.zero && _controller.isGrounded)
            {
                if(!footsteps.isPlaying)
                {
                    footsteps.pitch = UnityEngine.Random.Range(0.8f, 1.2f) + 1f;
                    footsteps.Play();
                }
            }
            //else if (footsteps.isPlaying) footsteps.Stop();
        }

        if (canLook && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            _xAngle += Input.GetAxis("Mouse X") * mouseSensetivity;
            _yAngle += Input.GetAxis("Mouse Y") * mouseSensetivity;
            _yAngle = Mathf.Clamp(_yAngle, -90, 90);

            transform.localRotation = Quaternion.Euler(0, _xAngle, 0);
            _mainCam.transform.parent.localRotation = Quaternion.Euler(-_yAngle, 0, 0);
        }
    }

    void HandleInteraction()
    {
        Texture selectedIcon = _inventory.GetHeldIcon();
        //openHandImage.texture = selectedIcon? selectedIcon : openHand;

        openHandImage.gameObject.SetActive(false);
        hoverText.text = "";

        RaycastHit raycast;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out raycast))
        {
            if (raycast.distance <= maxInteractDistance)
            {
                GameObject hit = raycast.collider.gameObject;
                Interactable i = hit.GetComponent<Interactable>();
                if (i == null && hit.transform.parent != null)
                {
                    hit = hit.transform.parent.gameObject;
                    i = hit.GetComponent<Interactable>();
                }
                if (i != null && i.canInteract)
                {
                    openHandImage.gameObject.SetActive(true);
                    hoverText.text = i.nameOnHover;
                    i.LookingAt();
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                        i.Interact();
                }
            }
        }
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }
    
    public void ToggleLook()
    {
        canLook = !canLook;
    }
}
