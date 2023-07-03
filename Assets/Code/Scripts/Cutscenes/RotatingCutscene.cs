using System.Collections;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class RotatingCutscene : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private CutScenePlayerRotator[] rotators;
        [SerializeField] private SpriteRenderer[] spriteRenderers;

        public void UpdateCirclePositions(int[] activePlayers, float[] desiredAngles)
        {
            if (enabled)
            {
                for (int i = 0; i < activePlayers.Length; i++)
                {
                    rotators[activePlayers[i]].StartRotationTowardsCircleAngle(desiredAngles[i]);
                }
                
                return;
            }
            
            for (int i = 0; i < activePlayers.Length; i++)
            {
                rotators[activePlayers[i]].SetInnerRotationInstant(desiredAngles[i]);
            }
        }
        
        public void AnimatePlayerEnteringCircle(int playerNumber)
        {
            rotators[playerNumber].StartRotationIntoCircle();
        }

        public void AnimatePlayerLeavingCircle(int playerNumber)
        {
            rotators[playerNumber].StartRotationOutOfCircle();
        }

        public IEnumerator AnimateAllPlayersEnteringTheCircle(int[] activePlayers)
        { 
            foreach (var player in activePlayers)
            {
                AnimatePlayerEnteringCircle(player);
            }

            var lastRotator = rotators[activePlayers[^1]];

            return new WaitWhile(lastRotator.IsRotating);
        }
        
        public IEnumerator AnimateAllPlayersLeavingTheCircle(int[] activePlayers)
        { 
            foreach (var player in activePlayers)
            {
                AnimatePlayerLeavingCircle(player);
            }

            var lastRotator = rotators[activePlayers[^1]];

            return new WaitWhile(lastRotator.IsRotating);
        }
        
        public void MoveAllPlayersOutsideTheCircle(int[] activePlayers)
        { 
            foreach (var player in activePlayers)
            {
                var rotator = rotators[player];
                rotator.RotateOutOfCircleInstant();
                rotator.SetInnerRotationInstant(0f);
            }
        }
        
        public void MoveAllPlayersIntoTheCircle(int[] activePlayers)
        { 
            foreach (var player in activePlayers)
            {
                var rotator = rotators[player];
                rotator.RotateIntoCircleInstant();
            }
        }

        private void Start()
        {
            for (int i = 0; i < 6; i++)
            {
                spriteRenderers[i].color = playerColors[i];
            }
        }
    }
}
