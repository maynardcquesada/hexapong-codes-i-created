using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _ballRb;
    [SerializeField] private float _ballSpeed;
    [SerializeField] private float _ballSpeedIncrease;
    private Vector2 _ballDirection;

    private void Start()
    {
        InitBallRandMovement();
    }

    public override void FixedUpdateNetwork()
    {
        _ballRb.MovePosition(_ballRb.position +
                             _ballDirection *
                             (_ballSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag($"Obstacles"))
        {
            _ballDirection = Vector2.Reflect(_ballDirection,
                col.GetContact(0).normal);
        }

        if (col.gameObject.CompareTag($"Paddle"))
        {
            _ballSpeed += _ballSpeedIncrease;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag($"Paddle"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
        }
    }

    private void InitBallRandMovement()
    {
        _ballDirection = Random.insideUnitCircle.normalized;
    }
}
