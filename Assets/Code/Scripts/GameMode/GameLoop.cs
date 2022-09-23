using System;
using System.Collections;
using Meyham.DataObjects;
using Meyham.Events;
using Meyham.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.GameMode
{
    public class GameLoop : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image timerUI;
        [SerializeField] private VoidEventChannelSO gameStartEvent, gameEndEvent, gameRestartEvent;
        [SerializeField] private VoidEventChannelSO endSpawningEvent, lastItemVanishedEvent;
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        [Header("Timer Values")]
        [SerializeField] private FloatValue currentTime;
        [SerializeField] private float timeUnit;
        private float lastTime, fillPerTimeUnit;

        [Header("Delays")]
        [SerializeField] private float restartButtonDelay;
        [SerializeField] private float endOfGameDelay;
        
        public void StartGame()
        {
            gameStartEvent.RaiseEvent();
            StartCoroutine(TimerRoutine());
        }

        private void AllowRestart()
        {
            Alerts.SendAlert("Drücken für Neustart");
            inputEventChannel += OnRestartPressed;
        }

        private void Awake()
        {
            fillPerTimeUnit = timeUnit / currentTime.BaseValue;
        }

        private void OnRestartPressed(int input)
        {
            gameRestartEvent.RaiseEvent();
            
            timerUI.fillAmount = 100f;
            currentTime.ResetToBaseValue();
            
            Alerts.ClearAlert();
            inputEventChannel -= OnRestartPressed;

            StartCoroutine(TimerRoutine());
        }

        private IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(timeUnit);

            timerUI.fillAmount -= fillPerTimeUnit;
            currentTime.RuntimeValue -= timeUnit;

            if (currentTime.RuntimeValue == 0f)
            {                
                lastItemVanishedEvent += OnLastItemVanished;
                endSpawningEvent.RaiseEvent();
                yield break;
            }

            StartCoroutine(TimerRoutine());
        }

        private void OnLastItemVanished()
        {
            lastItemVanishedEvent -= OnLastItemVanished;
            StartCoroutine(DelayedCall(endOfGameDelay, OnTimeElapsed));
        }

        private void OnTimeElapsed()
        {
            gameEndEvent.RaiseEvent();
            StartCoroutine(DelayedCall(restartButtonDelay, AllowRestart));
        }
        
        private IEnumerator DelayedCall(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }
    }
}
