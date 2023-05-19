using UnityEngine;
using Random = UnityEngine.Random;

namespace Power_Up
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject powerUpPrefab;
        [SerializeField] private Transform ball;
        [SerializeField] private int _arenaSize;
        private float _timer;

        private void Update()
        {
            ConditionsToSpawn();
        }

        private void ConditionsToSpawn()
        {
            if (_arenaSize > 1)
            {
                _timer += Time.deltaTime;
                if (!(_timer >= 5.0f) || powerUpPrefab.activeSelf) return;
                if (ball.position != Vector3.zero)
                {
                    if (Random.Range(1, 3) == 2)
                    {
                        powerUpPrefab.transform.position =
                            Random.insideUnitCircle * 2;
                        powerUpPrefab.SetActive(true);
                    }

                    _timer = 0;
                }

                _timer = 0;
            }
        }

        public void DetermineArenaSize()
        {
            _arenaSize--;
        }
    }
}
