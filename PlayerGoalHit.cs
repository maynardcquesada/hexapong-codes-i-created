using Fusion;
using UnityEngine;

public class PlayerGoalHit : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag($"Ball"))
        {
            print($"Hit");
        }
    }
}
