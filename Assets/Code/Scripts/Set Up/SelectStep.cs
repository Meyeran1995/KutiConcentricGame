﻿using System.Collections;
using Meyham.Cutscenes;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class SelectStep : AGameStep
    {
        [Header("Properties")] 
        [SerializeField] private int startDelay;
        
        [Header("References")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        [SerializeField] private PlayerColors colors;
        [SerializeField] private GameObject frontEnd;
        [SerializeField] private PlayerSelectionAnimator selectionAnimator;

        [Header("Debug")]
        [ReadOnly, SerializeField] private int numberOfPlayers;

        private bool[] activeSlots;
        private Coroutine delayedStartRoutine;

        private MainMenuView mainMenu;

        private IColoredText[] coloredTexts;
        private IPlayerNumberDependable[] playerNumberDependables;

        private PlayerManager playerManager;
        
        public override void SeTup()
        {
            mainMenu = FindAnyObjectByType<MainMenuView>();
            
            coloredTexts = frontEnd.GetComponentsInChildren<IColoredText>(true);

            var dependables = frontEnd.GetComponentsInChildren<IPlayerNumberDependable>();

            playerNumberDependables = new IPlayerNumberDependable[dependables.Length + 2];

            playerNumberDependables[1] = selectionAnimator;
            
            for (int i = 0; i < dependables.Length; i++)
            {
                playerNumberDependables[2 + i] = dependables[i];
            }
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
        }

        public override void Deactivate()
        {
            mainMenu.CloseView();
            
            playerManager.UpdatePlayerCount();
            playerManager.UpdatePlayerColors();
            
            base.Deactivate();
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
            playerNumberDependables[0] = manager;
        }

        private void OnEnable()
        {
            activeSlots = new bool[6];
            inputEventChannel += OnPlayerJoined;
            mainMenu.OpenView();
        }
        
        private void OnDisable()
        {
            inputEventChannel -= OnPlayerJoined;
            activeSlots = null;
        }

        private void OnPlayerJoined(int playerNumber)
        {
            if (activeSlots[playerNumber])
            {
                --numberOfPlayers;
                OnPlayerLeft(playerNumber);
                return;
            }

            SetColors(playerNumber);

            foreach (var dependable in playerNumberDependables)
            {
                dependable.OnPlayerJoined(playerNumber);
            }
            
            activeSlots[playerNumber] = true;
            
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
            foreach (var dependable in playerNumberDependables)
            {
                dependable.OnPlayerLeft(playerNumber);
            }
            
            activeSlots[playerNumber] = false;
            
            if(numberOfPlayers > 0 && delayedStartRoutine != null) return;
            
            StopCoroutine(delayedStartRoutine);
            delayedStartRoutine = null;
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
            
            delayedStartRoutine = null;
            StartGame();
        }

        private void SetColors(int playerNumber)
        {
            foreach (var text in coloredTexts)
            {
                text.SetTextColor(playerNumber, colors[playerNumber]);
            }
        }

        private void StartGame()
        {
            Alerts.ClearAlert();

            Deactivate();
            
            if(delayedStartRoutine == null) return;
            
            StopCoroutine(delayedStartRoutine);
        }
    }
}