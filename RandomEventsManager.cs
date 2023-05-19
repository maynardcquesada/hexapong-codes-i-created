using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;
//Will be back after implementing ball physics in main scene
namespace Event_Scripts
{
    public class RandomEventsManager : MonoBehaviour
    {
        private readonly IDictionary<int, Action> _randomEvents = new 
            Dictionary<int, Action>();

        private  bool _stillPlaying;
        private byte _arenaSize = 6;
        [SerializeField]private GameObject _warningScreen;

        #region Shapeshifting Paddles Properties
        
        private readonly Vector2 _shrinkSize = new Vector2(0.5f, 1);
        private readonly Vector2 _growSize = new Vector2(2f, 1);
        private readonly Vector2 _shrinkVision = new Vector2(2.2f, 1);

        #endregion

        #region Blink Properties

        [SerializeField] private PostProcessVolume postProcessVolumeForBlink;
        private Blur _blur;

        #endregion

        #region Frenzied Balls
        //for future iterations
        /*[SerializeField] private GameObject ballInstance;
        private List<GameObject> _instantiatedBalls = new List<GameObject>();
        [SerializeField] private CircleCollider2D ballCollider;
        private List<Collider2D> _touchingColliders = new List<Collider2D>();*/

        #endregion

        private void Awake()
        {
            InitializeBlurProfile();
        }

        private void Start()
        {
            InitializeRandomEvents();
            ChooseRandomEvent();
        }

        private void InitializeRandomEvents()
        {
            _randomEvents.Add(1, RotateArenaEvent);
            _randomEvents.Add(2, ShapeShiftingPaddlesEvent);
            _randomEvents.Add(3, BlinkEvent);
            //_randomEvents.Add(4, FrenziedBallsEvent); for future iterations
        }

        #region Rotate Arena
        
        private IEnumerator<float> RotateArena(GameObject arena, float 
            rotationDegree)
        {
            const float rotationTimeSpan = 8;
            const float rotationWait = 11;
            
            while (_stillPlaying)
            {
                var rotationWise = Random.Range(0, 2);
                if (rotationWise == 0) rotationDegree *= -1;
                LeanTween.rotateAround(arena, Vector3.forward, 
                    rotationDegree, rotationTimeSpan);
                
                yield return Timing.WaitForSeconds(rotationWait);
            }
        }
        
        private void RotateArenaEvent()
        {
            Timing.RunCoroutine(RotateArena(GameObject.FindWithTag("Arena"),
                RotationDegree()));
        }
        
        public void DetermineArenaSize()
        {
            _arenaSize--;
            if (_arenaSize < 1)
            {
                gameObject.SetActive(false);
            }
        }

        private float RotationDegree()
        {
            return _arenaSize switch
            {
                6 => 60,
                3 => 60,
                5 => 72,
                4 => 90,
                2 => 180,
                _ => 0
            };
        }

        #endregion

        #region Shapeshifting Paddles

        private void ShapeShiftingPaddlesEvent()
        {
            const float duration = 2f;
            for (var i = 0; i < 6; i++)
            {
                var randInt = Random.Range(0, 2);
                var tempPaddle = GameObject.FindWithTag($"Paddle_P{i + 1}");
                if (tempPaddle != null)
                {
                    var tempVision = tempPaddle.transform.GetChild(0).transform
                        .GetChild(7);
                    
                    switch (randInt)
                    {
                        case 0:
                            LeanTween.scale(tempPaddle, _shrinkSize, duration)
                                .setEase(LeanTweenType.easeOutQuad);
                            tempVision.transform.localScale = _shrinkVision;
                            break;
                        case 1:
                            LeanTween.scale(tempPaddle, _growSize, duration)
                                .setEase(LeanTweenType.easeOutQuad);
                            break;
                    }
                }
            }
        }

        #endregion

        #region Blink

        private void InitializeBlurProfile()
        {
            postProcessVolumeForBlink.profile.TryGetSettings(out _blur);
        }

        private IEnumerator<float> Blink()
        {
            while (_stillPlaying)
            {
                const float targetBlurIntensity = 6.78f;
                const float duration = 3f;
                const int blurIntensityOff = 0;
                
                BlurOn(targetBlurIntensity, duration);
                yield return Timing.WaitForSeconds(3);
                
                BlurOff(blurIntensityOff, duration);
                yield return Timing.WaitForSeconds(3);
            }
        }

        private void BlurOn(float targetBlurIntensity, float duration)
        {
            LeanTween
                .value(gameObject, UpdateBlink, _blur.BlurSize.value,
                    targetBlurIntensity, duration)
                .setEase(LeanTweenType.easeOutQuad);
        }
        
        private void BlurOff(float blurIntensityOff, float duration)
        {
            LeanTween
                .value(gameObject, UpdateBlink, _blur.BlurSize.value,
                    blurIntensityOff, duration)
                .setEase(LeanTweenType.easeOutQuad);
        }
        
        private void UpdateBlink(float value)
        {
            const int blurDataOffset = 3;
            _blur.Downsample.value = (int)value - blurDataOffset;
            _blur.BlurIterations.value = (int)value - blurDataOffset;
            _blur.BlurSize.value = value;
        }

        private void BlinkEvent()
        {
            Timing.RunCoroutine(Blink());
        }

        private void ResetBlinkEvent()
        {
            _blur.Downsample.value = 0;
            _blur.BlurIterations.value = 0;
            _blur.BlurSize.value = 0;
        }

        #endregion

        #region Frenzied Balls
        //for future iterations note: fixed IEnumerator as it is unending loop and causes crash
        /*private void FrenziedBallsEvent()
        {
            Timing.RunCoroutine(FrenziedBalls());
        }

        private IEnumerator<float> FrenziedBalls()
        {
            while (_stillPlaying)
            {
                ballCollider.GetContacts(_touchingColliders);
                foreach (var touchingCollider in _touchingColliders)
                {
                    print(touchingCollider);
                }
            }
            yield break;
        }*/

        #endregion

        public void ChooseRandomEvent()
        { 
            const int eventMinValue = 1;
            var randInt = Random.Range(eventMinValue, _randomEvents.Count + 1);
            ResetRandomEvent();
            Timing.RunCoroutine(CountdownForEvent(randInt));
        }
        
        private void ResetRandomEvent()
        {
            _stillPlaying = false;
            Timing.KillCoroutines();
            ResetBlinkEvent();
        }

        private IEnumerator<float> CountdownForEvent(int chosenEvent)
        {
            const int forCountdown = 6;
            const int forEvent = 5;
            const int forWarningEvent = 2;

            yield return Timing.WaitForSeconds(forCountdown);
            yield return Timing.WaitForSeconds(forEvent);
            _warningScreen.SetActive(true);
            yield return Timing.WaitForSeconds(forWarningEvent);
            _warningScreen.SetActive(false);
            _stillPlaying = true;
            _randomEvents[chosenEvent]();
        }
    }
}
