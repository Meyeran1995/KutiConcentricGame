using System.Collections;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class PlayerNumberSelector : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private float startButtonDelay;

        [Header("References")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        [SerializeField] private PlayerManager playerManager;
        
        [Header("Debug")]
        [ReadOnly, SerializeField] private int numberOfPlayers;
        [ReadOnly, SerializeField] private bool startButtonActive;

        private bool[] activePlayers;

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
                OnStartButton();
                return;
            }
            
            activePlayers[playerNumber] = true;
            playerManager.OnPlayerJoined(playerNumber);
            
            if(++numberOfPlayers != 1) return;

            StartCoroutine(DelayedStartButtonRegistration());
        }

        private IEnumerator DelayedStartButtonRegistration()
        {
            yield return new WaitForSeconds(startButtonDelay);

            startButtonActive = true;
        }

        private void OnStartButton()
        {
            if(!startButtonActive) return;
            
            inputEventChannel -= OnPlayerJoined;
            
            playerManager.OnGameStart(numberOfPlayers);
        }
    }
}