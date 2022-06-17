using System.Collections;
using Meyham.DataObjects;
using Meyham.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.GameMode
{
    public class EndTimer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image timerUI;
        [SerializeField] private VoidEventChannelSO collectionEvent;
        
        [Header("Values")]
        [SerializeField] private FloatValue currentTime;
        [SerializeField] private float timeUnit, fillPerTimeUnit;

        private float lastTime;
        
        private void Awake()
        {
        }

        private void Start()
        {
            collectionEvent += OnItemCollected;
            StartCoroutine(TimerRoutine());
        }

        private IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(timeUnit);

            timerUI.fillAmount -= fillPerTimeUnit;
            currentTime.RuntimeValue -= timeUnit;

            if (currentTime.RuntimeValue == 0f)
            {
                OnTimeElapsed();
                yield break;
            }

            StartCoroutine(TimerRoutine());
        }

        private void OnItemCollected()
        {
            UpdateTimerUI();
        }
        
        private void OnTimeElapsed()
        {
            Debug.Log("Game over");
        }

        private void UpdateTimerUI()
        {
            float newFill = currentTime.RuntimeValue / timeUnit * fillPerTimeUnit;
            timerUI.fillAmount = newFill;
        }
    }
}