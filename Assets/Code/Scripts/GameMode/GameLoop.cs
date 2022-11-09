using System;
using System.Collections;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.UI;
using UnityEngine;

namespace Meyham.GameMode
{
    public class GameLoop : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO gameStartEvent, gameEndEvent, gameRestartEvent;
        [SerializeField] private VoidEventChannelSO endSpawningEvent, lastItemVanishedEvent;
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        [Header("Timer Values")]
        [SerializeField] private FloatValue timeUnit;
        [SerializeField, ReadOnly] private float currentTime;

        [Header("Delays")]
        [SerializeField] private float restartButtonDelay;
        [SerializeField] private float endOfGameDelay;

        private float startingTime;
        
        private static TimerUi timerUi;

        public static void RegisterTimer(TimerUi timer)
        {
            timerUi = timer;
        }
        
        public void StartGame()
        {
            gameStartEvent.RaiseEvent();
            StartCoroutine(TimerRoutine());
        }

        private void Awake()
        {
            startingTime = TimerUi.NumberOfDots * timeUnit;
            currentTime = startingTime;
        }

        private void AllowRestart()
        {
            Alerts.SendAlert("Drücken für Neustart");
            inputEventChannel += OnRestartPressed;
        }

        private void OnRestartPressed(int input)
        {
            gameRestartEvent.RaiseEvent();
            
            timerUi.ResetTimer();
            currentTime = startingTime;
            
            Alerts.ClearAlert();
            inputEventChannel -= OnRestartPressed;

            StartCoroutine(TimerRoutine());
        }

        private IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(timeUnit);

            timerUi.DepleteTime();
            currentTime -= timeUnit;

            if (currentTime <= 0f)
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
