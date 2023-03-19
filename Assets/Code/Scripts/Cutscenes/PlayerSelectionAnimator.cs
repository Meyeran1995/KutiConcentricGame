using System.Collections;
using System.Collections.Generic;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class PlayerSelectionAnimator : MonoBehaviour, IPlayerNumberDependable
    {
        [Header("References")] 
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private CutScenePlayerRotator[] rotators;
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        
        private List<int> activePlayers;

        public int[] PlayerSelectionOrder()
        {
            return activePlayers.ToArray();
        }
        
        public void OnPlayerJoined(int playerNumber)
        {
            rotators[playerNumber].RotateIntoCircle();
            spriteRenderers[playerNumber].color = playerColors[playerNumber];
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

        public void CloseCutscene()
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = false;
            }

            StartCoroutine(WaitForElementsToLeaveCircle());
        }

        private void Awake()
        {
            activePlayers = new List<int>(6);
        }

        private void OnEnable()
        {
            activePlayers ??= new List<int>(6);

            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = true;
            }
        }

        private void OnDisable()
        {
            activePlayers = null;
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

        private IEnumerator WaitForElementsToLeaveCircle()
        {
            foreach (var player in activePlayers)
            {
                rotators[player].RotateOutOfCircle();
            }

            CutScenePlayerRotator lastRotator = rotators[activePlayers[^1]];

            yield return new WaitWhile(lastRotator.IsRotating);
            
            activePlayers.Clear();
            gameObject.SetActive(false);
        }
    }
}