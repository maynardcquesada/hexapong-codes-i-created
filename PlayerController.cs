using Fusion;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _playerPaddleRb;
    [SerializeField] private float _playerPaddlespeed;
    [SerializeField] private float _playerSpeedIncreaseOnHit;
    private Vector2 _playerTransform = new Vector2(0, 0);
    private Vector2 _directionInput;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            _directionInput.x = data.XDirection;
        }
        else
        {
            _directionInput = Vector2.zero;
        }

        var transformRight = transform.right;
        
        _playerTransform.x = transformRight.x;
        _playerTransform.y = transformRight.y;
        _playerPaddleRb.MovePosition(_playerPaddleRb.position +
                                     _playerTransform * _directionInput.x *
                                     _playerPaddlespeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag($"Ball"))
        {
            _playerPaddlespeed += _playerSpeedIncreaseOnHit;
        }
    }
}
