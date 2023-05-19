using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _ballRigidbody;
    [SerializeField] private float _ballSpeed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private SoundEffectsSO _ballHit;
    private Vector2 _ballDirection;
    private float _initialSpeed;
    private bool _move;

    void Start()
    {
        _initialSpeed = _ballSpeed;
        Invoke(nameof(LaunchStart), 4f);
    }
    
    void FixedUpdate()
    {
        if (_move)
        {
            _ballRigidbody.MovePosition(_ballRigidbody.position +
                                        _ballDirection *
                                        (_ballSpeed * Time.deltaTime));
        }
    }

    public void LaunchStart()
    {
        _ballDirection = Random.insideUnitCircle.normalized;
        _move = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag($"wall"))
        {
            _ballDirection = Vector2.Reflect(_ballDirection,
                collision.GetContact(0).normal);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag($"Paddle_P1"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
        if (col.transform.CompareTag($"Paddle_P2"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
        if (col.transform.CompareTag($"Paddle_P3"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
        if (col.transform.CompareTag($"Paddle_P4"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
        if (col.transform.CompareTag($"Paddle_P5"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
        if (col.transform.CompareTag($"Paddle_P6"))
        {
            _ballDirection = col.transform.GetChild(0).transform.up;
            _ballSpeed += _speedMultiplier;
            _ballHit.Play();
        }
    }

    public void SlowMoSpeed(float percent) => _ballSpeed *= percent / 100;
    public void ResetSpeed() => _ballSpeed = _initialSpeed;
    public void ResetBall()
    {
        transform.position = Vector3.zero;
        Invoke(nameof(LaunchStart), 4f);
        _move = false;
    }
}
