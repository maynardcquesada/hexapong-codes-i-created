using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake() { current = this; }
    

    
    public event Action<byte> OnCurrentPlayerIsAI;
    public void CurrentPlayerIsAI(byte id) { OnCurrentPlayerIsAI?.Invoke(id); }
    
    

    public event Action<byte, byte> OnAIMovementDirection;
    public void AIMovementDirection(byte visionID, byte direction) 
    { 
        OnAIMovementDirection?.Invoke(visionID, direction);
    }
}
