using System.Collections;
using Meyham.Collision;
using Meyham.Cutscenes;
using Meyham.GameMode;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class StartStep : AGameStep
    {
        [SerializeField] private RotatingCutscene rotatingCutscene;

        private PlayerSelectionAnimator playerSelection;

        private PlayerCollisionResolver collisionResolver;

        private PlayerManager playerManager;

        private PlayerStartingPositionProvider startingPositionsProvider;

        private int numberOfPlayers;

        public override void Setup()
        {
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkPlayerSelectionAnimation(LinkSelectionAnimation);
            loop.LinkPlayerCollisionResolver(LinkCollisionResolver);
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForSetUp());
        }

        private void LinkCollisionResolver(PlayerCollisionResolver resolver)
        {
            collisionResolver = resolver;
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }
        
        private void LinkSelectionAnimation(PlayerSelectionAnimator playerSelectionAnimation)
        {
            playerSelection = playerSelectionAnimation;
        }

        private void SetPlayersToStartingPositions(PlayerController[] players)
        {
            startingPositionsProvider.RotateStartingPositions();
            
            for (int i = 0; i < players.Length; i++)
            {
                players[i].SetStartingPosition(startingPositionsProvider.GetStartingPosition(i));
            }
        }

        private void GameSetUp(PlayerController[] players)
        {
            numberOfPlayers = players.Length;
            startingPositionsProvider = new PlayerStartingPositionProvider(players.Length);
            
            var collisions = new PlayerCollision[numberOfPlayers];
            var playerNumbersInOrder = playerSelection.PlayerSelectionOrder();

            for (int i = 0; i < players.Length; i++)
            {
                PlayerController player = players[0];
                var currentDesignation = (PlayerDesignation)playerNumbersInOrder[i];

                foreach (var controller in players)
                {
                    if (controller.Designation != currentDesignation)
                    {
                        continue;
                    }

                    player = controller;
                }
                
                player.SetStartingPosition(startingPositionsProvider.GetStartingPosition(i));
                collisions[i] = player.GetComponentInChildren<PlayerCollision>(true);
            }
            
            collisionResolver.SetPlayerCollisions(collisions);
        }
        
        private IEnumerator WaitForSetUp()
        {
            var players = playerManager.GetPlayers();

            if (numberOfPlayers != players.Length)
            {
                GameSetUp(players);
                Deactivate();
                yield break;
            }
            
            SetPlayersToStartingPositions(players);

            var playerIDs = new int[numberOfPlayers];
            var playerAngles = new float[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerIDs[i] = (int)players[i].Designation;
                playerAngles[i] = players[i].GetCurrentCirclePosition();
            }

            rotatingCutscene.gameObject.SetActive(true);
            rotatingCutscene.UpdateCirclePositions(playerIDs, playerAngles);

            yield return rotatingCutscene.AnimateAllPlayersEnteringTheCircle(playerIDs);

            rotatingCutscene.gameObject.SetActive(false);
            playerManager.ShowPlayers();

            Deactivate();
        }
    }
}