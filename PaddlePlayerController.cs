using UnityEngine;
using UnityEngine.InputSystem;

public class PaddlePlayerController : MonoBehaviour
{
    [SerializeField] private byte _id;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private float _playerSpeed;
    private bool _anAI;
    
    private PlayerControls _input;
    private float _direction;
    private Vector3 _targetLocalPos;
    
    private InputAction _movement;
    private Vector2 _playerTransform = new Vector2(0, 0);

    public enum Player
    {
        player1,
        player2,
        player3,
        player4,
        player5,
        player6,
        AI,
    }
    public Player currentPlayer;
    [SerializeField] private float _playerSpeedIncreaseOnHit;

    private void Awake()
    {
        _input = new PlayerControls();

        PlayerSwitchInput();
    }

    private void OnDisable()
    {
        if (_anAI) return;;
        _movement.Disable();
        _movement.started -= Move;
    }
    
    private void Start()
    {
        if (_anAI)
        {
            GameEvents.current.CurrentPlayerIsAI(_id);
        }
    }

    private void FixedUpdate()
    {
        var transformRight = transform.right;
        
        _playerTransform.x = transformRight.x;
        _playerTransform.y = transformRight.y;
        _playerRigidBody.MovePosition(_playerRigidBody.position +
                                      _playerTransform * (_direction *
                                          _playerSpeed * Time.fixedDeltaTime));
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag($"ball"))
        {
            _playerSpeed += _playerSpeedIncreaseOnHit;
        }
    }
    
    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _direction = context.ReadValue<float>();
        }
        
        if (context.canceled)
        {
            _direction = 0;
        }
    }

    private void PlayerSwitchInput()
    {
        switch (currentPlayer)
        {
            case Player.player1:
                _movement = _input.Player1.Movement;
                break;
            case Player.player2:
                _movement = _input.Player2.Movement;
                break;
            case Player.player3:
                _movement = _input.Player3.Movement;
                break;
            case Player.player4:
                _movement = _input.Player4.Movement;
                break;
            case Player.player5:
                _movement = _input.Player5.Movement;
                break;
            case Player.player6:
                _movement = _input.Player6.Movement;
                break;
            case Player.AI:
                _anAI = true;
                break;
        }
        MovementEventRegistration();
    }

    private void MovementEventRegistration()
    {
        if (_anAI) return;
        _movement.Enable();
        _movement.performed += Move;
        _movement.canceled += Move;
    }
}
