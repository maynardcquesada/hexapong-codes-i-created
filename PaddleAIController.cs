using UnityEngine;

public class PaddleAIController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private byte _id;
    
    private bool _anAI;
    private byte _direction;

    private void Awake()
    {
        GameEvents.current.OnCurrentPlayerIsAI += CatchCurrentPlayerIsAI;
        GameEvents.current.OnAIMovementDirection += CatchAIMovementDirection;
    }

    private void Update()
    {
        AIMovement();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnCurrentPlayerIsAI -= CatchCurrentPlayerIsAI;
        GameEvents.current.OnAIMovementDirection -= CatchAIMovementDirection;
    }
    
    private void AIMovement()
    {
        if (!_anAI) return;
        var step = Time.deltaTime * _speed;
        switch (_direction)
        {
            case 0:
            { 
                transform.Translate(Vector3.zero);
                break;
            }
            case 1:
            {
                transform.Translate(Vector3.left * step);
                break;
            }
            case 2:
            {
                transform.Translate(Vector3.right * step);
                break;
            }
        }
    }

    private void CatchCurrentPlayerIsAI(byte id)
    {
        if (id == _id)
        {
            _anAI = true;   
        }
    }

    private void CatchAIMovementDirection(byte visionID, byte direction)
    {
        if (_id == visionID)
        {
            _direction = direction;
        }
    }
}
