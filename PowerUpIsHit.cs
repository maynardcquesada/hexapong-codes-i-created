using UnityEngine;

public class PowerUpIsHit : MonoBehaviour
{
    [SerializeField] private VoidEvent powerUpIsHit;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ball"))
        {
            powerUpIsHit.Raise();
        }
    }
}
