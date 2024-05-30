using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public partial class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    Vector3 beforeDirection;
    public float jumpPower;
    public float loseStamina;
    public LayerMask groundLayerMask;
    [Header("Look")]
    public Transform cameraContainer;
    public Camera cameraFPS;
    public Camera cameraTPS;
    public float minXLook;
    public float maxXLook;
    public bool dash;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;
    public int selectItemIndex;

    public System.Action<int> inventory;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        ActivateCamera(cameraFPS);
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        if (dash) dir *=2;
        dir.y = _rigidbody.velocity.y;

        if(dir != Vector3.zero)
        {
            _rigidbody.velocity = dir;
            beforeDirection = dir;
        }
        else
        {
            if(dir != beforeDirection)
            {
                _rigidbody.velocity = dir;
                beforeDirection = dir;
            }
        }
        Debug.Log(_rigidbody.velocity);

        //_rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }
    public void OnView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (cameraFPS.isActiveAndEnabled)
            {
                cameraTPS.enabled = true;
                cameraFPS.enabled = false;
            }
            else
            {
                cameraTPS.enabled = false;
                cameraFPS.enabled = true;
            }

        }

    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CharacterManager.Instance.Player.condition.UseSkill(loseStamina);
            if(!CharacterManager.Instance.Player.condition.staminaOring)
                dash = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            CharacterManager.Instance.Player.condition.UseSkill(-loseStamina);
            dash = false;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
            mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isGrounded() && CharacterManager.Instance.Player.condition.UseStamina(loseStamina))
                _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnItemKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            var key = context.control.name;
            if (int.TryParse(key, out int index))
            {
                selectItemIndex = index;
                inventory?.Invoke(selectItemIndex);
            }
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f)+ transform.up*0.01f,Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)+ transform.up*0.01f,Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f)+ transform.up*0.01f,Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f)+ transform.up*0.01f,Vector3.down)
        };
        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    void ActivateCamera(Camera cam)
    {
        Camera[] allCameras = Camera.allCameras;
        foreach (Camera c in allCameras)
        {
            c.enabled = false;
        }
        cam.enabled = true;
    }

}
