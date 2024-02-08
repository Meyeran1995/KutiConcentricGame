using System.Collections;
using System.Collections.Generic;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Animation
{
    public class RotatingCutscene : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private CutScenePlayerRotator[] rotators;
        
        private IPlayerColorReceiver[] cutsceneDummyRenderers;

        public void UpdateCirclePositions(int[] activePlayers, float[] desiredAngles)
        {
            if (gameObject.activeSelf)
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
        
        public void MoveAllPlayersOutsideTheCircleInstant(IEnumerable<int> activePlayers)
        { 
            foreach (var player in activePlayers)
            {
                var rotator = rotators[player];
                rotator.RotateOutOfCircleInstant();
                rotator.SetInnerRotationInstant(0f);
            }
        }

        private void Start()
        {
            cutsceneDummyRenderers = new IPlayerColorReceiver[rotators.Length];

            for (int i = 0; i < rotators.Length; i++)
            {
                var dummyRenderer = rotators[i].GetComponentInChildren<IPlayerColorReceiver>();
                
                dummyRenderer.SetColor(i, playerColors[i]);
                cutsceneDummyRenderers[i] = dummyRenderer;
            }
        }
    }
}
