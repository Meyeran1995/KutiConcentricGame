using System;
using System.Collections.Generic;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class PlayerSelectionAnimator : MonoBehaviour, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private CutScenePlayerRotator[] rotators;

        private List<int> activePlayers;
        
        public void OnPlayerJoined(int playerNumber)
        {
            rotators[playerNumber].RotateIntoCircle();
            activePlayers.Add(playerNumber);
            
            UpdateCirclePositions();
        }

        public void OnPlayerLeft(int playerNumber)
        {
            rotators[playerNumber].RotateOutOfCircle();
            activePlayers.Remove(playerNumber);

            if (activePlayers.Count == 0) return;
            
            UpdateCirclePositions();
        }

        private void Awake()
        {
            activePlayers = new List<int>(6);
        }

        private void UpdateCirclePositions()
        {
            var desiredAngle = 0f;
            var numOfPlayers = activePlayers.Count;
            var angleIncrement = Mathf.RoundToInt(360f / numOfPlayers);

            for (int i = 0; i < numOfPlayers; i++)
            {
                rotators[activePlayers[i]].RotateTowardsCircleAngle(desiredAngle);
                desiredAngle += angleIncrement;
            }
        }
    }
}