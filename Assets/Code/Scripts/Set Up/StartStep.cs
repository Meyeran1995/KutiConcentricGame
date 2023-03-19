using System.Collections;
using Meyham.Collision;
using Meyham.Cutscenes;
using Meyham.GameMode;
using Meyham.Player;
using Meyham.UI;

namespace Meyham.Set_Up
{
    public class StartStep : AGameStep
    {
        private InGameView inGameView;

        private PlayerSelectionAnimator playerSelection;

        private PlayerCollisionResolver collisionResolver;

        private PlayerManager playerManager;

        private PlayerStartingPositionProvider startingPositionsProvider;

        private int numberOfPlayers;

        public override void SeTup()
        {
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkInGameView(LinkView);
            loop.LinkPlayerSelectionAnimation(LinkSelectionAnimation);
            loop.LinkPlayerCollisionResolver(LinkCollisionResolver);
        }
        
        public override void Activate()
        {
            base.Activate();

            StartCoroutine(WaitForViewToOpen());
        }
        
        private void LinkCollisionResolver(PlayerCollisionResolver resolver)
        {
            collisionResolver = resolver;
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }
        
        private void LinkView(InGameView gameView)
        {
            inGameView = gameView;
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

        private void GameSetUp()
        {
            var players = playerManager.GetPlayers();

            if (numberOfPlayers == players.Length)
            {
                SetPlayersToStartingPositions(players);
                return;
            }

            numberOfPlayers = players.Length;
            startingPositionsProvider = new PlayerStartingPositionProvider(players.Length);
            
            var collisions = new PlayerCollision[numberOfPlayers];
            var playerIndicesInOrder = playerSelection.PlayerSelectionOrder();

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[playerIndicesInOrder[i]];
                player.SetStartingPosition(startingPositionsProvider.GetStartingPosition(i));
                collisions[i] = player.GetComponentInChildren<PlayerCollision>(true);
            }
            
            collisionResolver.SetPlayerCollisions(collisions);
        }
        
        private IEnumerator WaitForViewToOpen()
        {
            GameSetUp();

            yield return inGameView.OpenView();
            
            Deactivate();
        }
    }
}