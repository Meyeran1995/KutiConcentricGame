using System.Collections;
using Meyham.DataObjects;
using Meyham.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.GameMode
{
    public class GameLoop : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image timerUI;
        [SerializeField] private VoidEventChannelSO gameStartEvent, gameEndEvent, gameRestartEvent;
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        [Header("Timer Values")]
        [SerializeField] private FloatValue currentTime;
        [SerializeField] private float timeUnit;
        private float lastTime, fillPerTimeUnit;
        
        public void StartGame()
        {
            gameStartEvent.RaiseEvent();
            StartCoroutine(TimerRoutine());
        }
        
        public void AllowRestart()
        {
            inputEventChannel += OnRestartPressed;
        }

        private void Awake()
        {
            fillPerTimeUnit = timeUnit / currentTime.BaseValue;
        }

        private void OnRestartPressed(int input)
        {
            gameRestartEvent.RaiseEvent();
            inputEventChannel -= OnRestartPressed;
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
        
        private void OnTimeElapsed()
        {
            gameEndEvent.RaiseEvent();
        }
    }
}
