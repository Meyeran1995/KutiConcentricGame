using System.Collections;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class PlayerNumberSelector : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private int startDelay;

        [Header("References")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        [SerializeField] private GameLoop gameLoop;
        
        [Header("Debug")]
        [ReadOnly, SerializeField] private int numberOfPlayers;

        private bool[] activePlayers;
        private Coroutine delayedStartRoutine;

        private void Awake()
        {
            activePlayers = new bool[6];
        }

        private void Start()
        {
            inputEventChannel += OnPlayerJoined;
        }

        private void OnPlayerJoined(int playerNumber)
        {
            if (activePlayers[playerNumber])
            {
                --numberOfPlayers;
                OnPlayerLeft(playerNumber);
                return;
            }
            
            activePlayers[playerNumber] = true;
            ++numberOfPlayers;

            switch (numberOfPlayers)
            {
                case 1:
                    if(delayedStartRoutine != null) return;
                    delayedStartRoutine = StartCoroutine(DelayedStart());
                    return;
                case 6:
                    StartGame();
                    return;
                default:
                    return;
            }
        }

        private void OnPlayerLeft(int playerNumber)
        {
            activePlayers[playerNumber] = false;
            
            if(numberOfPlayers > 0 && delayedStartRoutine != null) return;
            
            StopCoroutine(delayedStartRoutine);
            Alerts.SendAlert("Wählt Eure Farben aus.");
        }

        private IEnumerator DelayedStart()
        {
            var alertPrefix = Alerts.GetCurrentAlert();

            for (int i = startDelay; i > 0; i--)
            {
                Alerts.SendAlert($"{alertPrefix}\nNoch {i} Sekunden...");
                yield return new WaitForSeconds(1f);
            }

            StartGame();
            delayedStartRoutine = null;
        }

        private void StartGame()
        {
            inputEventChannel -= OnPlayerJoined;
            
            Alerts.ClearAlert();

            gameLoop.StartGame();
            
            if(delayedStartRoutine == null) return;
            
            StopCoroutine(delayedStartRoutine);
        }
    }
}