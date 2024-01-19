using System;
using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.GameMode;
using UnityEditor;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField] private FloatParameter radius;
        [SerializeField] private float anglePerBodyPart;
        [SerializeField] private RadialPlayerMovement playerMovement;

        [ReadOnly, SerializeField] private bool headIsFront = true;

        private float counter;
        
        // verwaltet body parts mit linked list
            // parts instanziieren/aus pool anfordern
            // parts abstoßen von einschlag aus
        // updatet alle body parts mit collision basierend auf der gemittelten order
        // setzt positionen von body parts

        private LinkedList<PlayerBodyPart> playerBodyParts = new();

        private static PlayerBodyPartPool bodyPartPool;

        public void AcquireBodyPart()
        {
            var incomingPart = bodyPartPool.GetBodyPart();
            
            if (headIsFront)
            {
                playerBodyParts.AddLast(incomingPart);
            }
            else
            {
                playerBodyParts.AddFirst(incomingPart);
            }

            AlignBodyPart(incomingPart);
        }

        public void LoseBodySegment()
        {

        }
        
        private void Awake()
        {
            playerBodyParts= new LinkedList<PlayerBodyPart>(GetComponentsInChildren<PlayerBodyPart>());
            bodyPartPool ??= FindAnyObjectByType<PlayerBodyPartPool>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            AlignBodyParts();
        }

        private void AlignBodyParts()
        {
            var index = 0;

            foreach (var bodyPart in playerBodyParts)
            {
                var angle = anglePerBodyPart * index;
                AlignBodyPart(index, bodyPart);
                index++;
            }
        }

        private void AlignBodyPart(int index, PlayerBodyPart bodyPart)
        {
            
            //what to do if tail is current head?
            //use max index - 1?
            
            var transformSelf = transform;
            var angle = anglePerBodyPart * index - 1 + playerMovement.CirclePosition;

            if (angle > 360f)
            {
                angle -= 360f;
            }
            
            var circlePos = GetCirclePoint(angle);

            circlePos = transformSelf.worldToLocalMatrix * circlePos;
            circlePos.y += radius;
            
            var partTransform = bodyPart.transform;
            partTransform.parent = transformSelf;
            partTransform.SetLocalPositionAndRotation(circlePos, Quaternion.AngleAxis(angle, Vector3.forward));
        }
        
        private void AlignBodyPart(PlayerBodyPart bodyPart)
        {
            AlignBodyPart(playerBodyParts.Count - 1, bodyPart);
        }
        
        private Vector3 GetCirclePoint(float currentAngle)
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);
        
            return new Vector3(x, y, 0f);
        }

#if UNITY_EDITOR

        [SerializeField] private int numberOfBodyParts;
        [SerializeField] private Vector3 gizmoScale;
        
        private void OnDrawGizmos()
        {
            var positionOffset = Vector3.zero;

            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                positionOffset = new Vector3(0f, radius);
            }
            
            for (int i = 0; i < numberOfBodyParts; i++)
            {
                var angle = i * anglePerBodyPart;
                var position = GetCirclePoint(angle) + positionOffset;
                Gizmos.DrawCube(position, gizmoScale);
            }
        }

#endif
    }
}
