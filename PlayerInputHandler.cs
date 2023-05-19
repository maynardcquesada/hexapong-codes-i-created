using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputVector = Vector2.zero;
    private NetworkInputData _networkInputData;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        SubscribeInputAction();
    }

    private void OnDisable()
    {
        UnsubscribeInputAction();
    }
    
    private void Start()
    {
        _networkInputData = new NetworkInputData();
    }

    private void Update()
    {
        PhoneMovement();
    }
    
    public NetworkInputData GetNetworkInput()
    {
        _networkInputData.XDirection = _inputVector.x;

        return _networkInputData;
    }
    
    private void ButtonBasedMovement(InputAction.CallbackContext context)
    {
        _inputVector = context.performed
            ? context.ReadValue<Vector2>()
            : Vector2.zero;
    }
    
    private void PhoneMovement()
    {
        if (!Application.isMobilePlatform) return;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            var touchPosition = touch.position;

            _inputVector = touchPosition.x < Screen.width / 2f
                ? Vector2.left
                : Vector2.right;
        }
        else
        {
            _inputVector = Vector2.zero;
        }
    }

    private void SubscribeInputAction()
    {
        _playerInputActions.Player.Movement.performed += ButtonBasedMovement;
        _playerInputActions.Player.Movement.canceled += ButtonBasedMovement;
        _playerInputActions.Player.Movement.Enable();
    }
    
    private void UnsubscribeInputAction()
    {
        _playerInputActions.Player.Movement.performed -= ButtonBasedMovement;
        _playerInputActions.Player.Movement.canceled -= ButtonBasedMovement;
        _playerInputActions.Player.Movement.Disable();
    }
}
