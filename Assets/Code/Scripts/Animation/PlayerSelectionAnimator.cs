using System.Collections.Generic;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Animation
{
    public class PlayerSelectionAnimator : MonoBehaviour, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private RotatingCutscene cutscene;
        
        private static List<int> activePlayers = new();

        public int[] PlayerSelectionOrder()
        {
            return activePlayers.ToArray();
        }
        
        public void OnPlayerJoined(int playerNumber)
        {
            cutscene.AnimatePlayerEnteringCircle(playerNumber);
            activePlayers.Add(playerNumber);
            
            UpdateCirclePositions();
        }

        public void OnPlayerLeft(int playerNumber)
        {
            cutscene.AnimatePlayerLeavingCircle(playerNumber);
            activePlayers.Remove(playerNumber);

            if (activePlayers.Count == 0) return;
            
            UpdateCirclePositions();
        }

        public void CloseCutscene()
        {
            cutscene.MoveAllPlayersOutsideTheCircleInstant(activePlayers.ToArray());
            cutscene.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            activePlayers = null;
        }

        private void UpdateCirclePositions()
        {
            var numOfPlayers = activePlayers.Count;
            var desiredAngles = new float[numOfPlayers];
            var desiredAngle = 0f;
            var angleIncrement = Mathf.RoundToInt(360f / numOfPlayers);

            for (int i = 0; i < numOfPlayers; i++)
            {
                desiredAngles[i] = desiredAngle;
                desiredAngle += angleIncrement;
            }
            
            cutscene.UpdateCirclePositions(activePlayers.ToArray(), desiredAngles);
        }
    }
}