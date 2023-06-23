using System.Collections;
using System.Collections.Generic;
using Meyham.Collision;
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
        [SerializeField] private PlayerColors colors;
        [SerializeField] private GameObject frontEnd;
        
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        [SerializeField] private GenericEventChannelSO<bool> setHoldInteractionEventChannel;

        [Header("Debug")]
        [ReadOnly, SerializeField] private int numberOfPlayers;

        private bool[] activeSlots;
        private Coroutine delayedStartRoutine;

        private MainMenuView mainMenu;

        private IColoredText[] coloredTexts;
        private List<IPlayerNumberDependable> playerNumberDependables;

        private PlayerManager playerManager;
        private PlayerSelectionAnimator playerSelection;

        public override void Setup()
        {
            activeSlots = new bool[6];
            coloredTexts = frontEnd.GetComponentsInChildren<IColoredText>(true);

            var dependables = frontEnd.GetComponentsInChildren<IPlayerNumberDependable>(true);

            playerNumberDependables = new List<IPlayerNumberDependable>(dependables);
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkMainMenuView(LinkView);
            loop.LinkPlayerSelectionAnimation(LinkSelectionAnimation);
            loop.LinkPlayerCollisionResolver(LinkCollisionResolver);
        }

        private void LinkCollisionResolver(PlayerCollisionResolver resolver)
        {
            playerNumberDependables.Add(resolver);
        }

        public override void Deactivate()
        {
            playerManager.UpdatePlayerCount();
            playerManager.UpdatePlayerColors();
            
            StartCoroutine(WaitForViewToClose());
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
            playerNumberDependables.Add(manager);
        }
        
        private void LinkView(MainMenuView mainMenuView)
        {
            mainMenu = mainMenuView;
        }

        private void LinkSelectionAnimation(PlayerSelectionAnimator playerSelectionAnimation)
        {
            playerSelection = playerSelectionAnimation;
            playerNumberDependables.Add(playerSelectionAnimation);
        }
        
        private void OnEnable()
        {
            setHoldInteractionEventChannel.RaiseEvent(false);
            StartCoroutine(DelayedEnable());
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

        private IEnumerator DelayedEnable()
        {
            yield return mainMenu.OpenView();
            inputEventChannel += OnPlayerJoined;
            Alerts.SendAlert("Wählt Eure Farben aus.");
        }

        private IEnumerator WaitForViewToClose()
        {
            inputEventChannel -= OnPlayerJoined;

            yield return mainMenu.CloseView();
            
            playerSelection.CloseCutscene();
            
            base.Deactivate();
            Destroy(this);
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