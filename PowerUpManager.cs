using System.Collections.Concurrent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Power_Up
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject ball;
        private Vector3 _randomBallRotation = new Vector3(0, 0, 0);
        private readonly ConcurrentBag<GameObject> _ballPool = new ConcurrentBag<GameObject>();
        private GameObject _ballInstance;
        private const int BallPoolSize = 1;

        private void OnValidate()
        {
            ball = GameObject.FindWithTag("ball");
        }

        private void Start()
        {
            PopulatePool();
        }

        public void ActivatePowerUps()
        {
            switch (Random.Range(1, 3))
            {
                case 1:
                    Sike();
                    break;
                case 2:
                    Twice();
                    break;
            }
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void Sike()
        {
            _randomBallRotation.x = Random.Range(1, 361);
            ball.transform.rotation = Quaternion.Euler(_randomBallRotation);
        }

        private void Twice()
        {
            _ballInstance = GetBallFromPool();
            _ballInstance.transform.position = Vector3.zero;
            var ballPhysics = _ballInstance.GetComponent<BallPhysics>();
            ballPhysics.LaunchStart();
        }

        public void ReturnBallInstance()
        {
            if (_ballInstance == null) return;
            _ballInstance.transform.position = Vector3.zero;
            ReturnBallToPool(_ballInstance);
        }

        private void PopulatePool()
        {
            for (var i = 0; i < BallPoolSize; i++)
            {
                var obj = Instantiate(ball);
                obj.SetActive(false);
                _ballPool.Add(obj);
            }
        }

        private GameObject GetBallFromPool()
        {
            GameObject obj;
            if (!_ballPool.TryTake(out obj))
            {
                obj = Instantiate(ball);
            }
            obj.SetActive(true);
            return obj;
        }

        private void ReturnBallToPool(GameObject obj)
        {
            obj.SetActive(false);
            _ballPool.Add(obj);
        }
    }
}
