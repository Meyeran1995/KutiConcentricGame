using System;
using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using UnityEditor;
using UnityEngine;

namespace Meyham.Player.Bodies
{
    
    // verwaltet body parts mit linked list
    // parts instanziieren/aus pool anfordern
    // parts abstoßen von einschlag aus
    // updatet alle body parts mit collision basierend auf der gemittelten order
    // setzt positionen von body parts
    
    public class PlayerBody : MonoBehaviour
    {
        public const int MIN_NUMBER_OF_BODY_PARTS = 3;
        
        public const float ANGLE_PER_BODY_PART = 12f;
        
        private const int max_number_of_body_parts = 30;
        
        [Header("Parameter")]
        [SerializeField] private FloatParameter radius;
        [Header("References")]
        [SerializeField] private RadialPlayerMovement playerMovement;
        [SerializeField] private GenericEventChannelSO<int> playerDestroyed;

        [ReadOnly, SerializeField] private bool headIsFront;
        [ReadOnly, SerializeField] private int hydraIndex;
        [ReadOnly, SerializeField] private PlayerDesignation designation;

        private bool initialized;
        
        private LinkedList<BodyPart> playerBodyParts = new();

        private static PlayerBodyPartPool bodyPartPool;

        public event Action<BodyPart> BodyPartAcquired;

        public void OnDoubleTap(int input)
        {
            headIsFront = input < 0;
        }

        public BodyPart[] GetBodyParts()
        {
            BodyPart[] bodyPartsBuffer = new BodyPart[playerBodyParts.Count];
            playerBodyParts.CopyTo(bodyPartsBuffer, 0);

            if (headIsFront)
            {
                return bodyPartsBuffer;
            }
            
            Array.Reverse(bodyPartsBuffer);
            
            return bodyPartsBuffer;
        }
        
        public void AcquireBodyPart()
        {
            if (!enabled) return;
            
            var incomingPart = bodyPartPool.GetBodyPart();
            
            if (headIsFront)
            {
                playerBodyParts.AddLast(incomingPart);
            }
            else
            {
                playerBodyParts.AddFirst(incomingPart);
                hydraIndex++;
            }

            AlignBodyPart(incomingPart);
            
            BodyPartAcquired?.Invoke(incomingPart);
        }

        public void LoseBodySegment()
        {
            BodyPart bodyPart;

            if (headIsFront)
            {
                bodyPart = playerBodyParts.Last.Value;
                playerBodyParts.RemoveLast();
            }
            else
            {
                bodyPart = playerBodyParts.First.Value;
                playerBodyParts.RemoveFirst();
                hydraIndex--;
            }

            bodyPartPool.ReleaseBodyPart(bodyPart.gameObject);
            
            if (transform.childCount > 0) return;
            
            playerDestroyed.RaiseEvent((int)designation);
        }
        
        private void Awake()
        {
            playerBodyParts = new LinkedList<BodyPart>();
            bodyPartPool ??= FindAnyObjectByType<PlayerBodyPartPool>(FindObjectsInactive.Include);
            designation = GetComponentInParent<PlayerController>().Designation;
            
            headIsFront = true;
            enabled = false;
        }

        private void OnEnable()
        {
            if (playerBodyParts.Count != 0)
            {
                foreach (var bodyPart in playerBodyParts)
                {
                    bodyPart.Show();
                }
                
                return;
            }
            
            for (int i = 0; i < MIN_NUMBER_OF_BODY_PARTS; i++)
            {
                AcquireBodyPart();
            }

            initialized = true;
        }

        private void OnDisable()
        {

#if UNITY_EDITOR
            if (isQuitting)
            {
                return;
            }
#endif
            
            if(!initialized) return;
            
            headIsFront = true;
            hydraIndex = 0;
            
            for (int i = 0; i < MIN_NUMBER_OF_BODY_PARTS; i++)
            {
                var incomingPart = bodyPartPool.GetBodyPart();
                playerBodyParts.AddLast(incomingPart);
                AlignBodyPart(incomingPart);
                incomingPart.Hide();
            }
        }

        private void AlignBodyPart(int index, BodyPart bodyPart)
        {
            var transformSelf = transform;
            var angle = ANGLE_PER_BODY_PART * index + playerMovement.CirclePosition;

            if (angle > 360f)
            {
                angle -= 360f;
            }
            
            var circlePos = GetCirclePoint(angle);
            circlePos = transformSelf.worldToLocalMatrix * circlePos;
            circlePos.y += radius;
            
            var localRotation = Quaternion.Inverse(transformSelf.rotation);
            localRotation *= Quaternion.AngleAxis(angle + 90f, Vector3.forward);
            
            var partTransform = bodyPart.transform;
            partTransform.parent = transformSelf;
            partTransform.SetLocalPositionAndRotation(circlePos, localRotation);
        }
        
        private void AlignBodyPart(BodyPart bodyPart)
        {
            var index = headIsFront ? playerBodyParts.Count - 1 : max_number_of_body_parts;
            
            AlignBodyPart(index - hydraIndex, bodyPart);
        }
        
        private Vector3 GetCirclePoint(float currentAngle)
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);
        
            return new Vector3(x, y, 0f);
        }

#if UNITY_EDITOR

        [SerializeField] private Vector3 gizmoScale;

        private bool isQuitting;
        
        private void OnDrawGizmos()
        {
            var positionOffset = Vector3.zero;

            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                positionOffset = new Vector3(0f, radius);
            }
            
            for (int i = 0; i < max_number_of_body_parts; i++)
            {
                var angle = i * ANGLE_PER_BODY_PART;
                var position = GetCirclePoint(angle) + positionOffset;
                Gizmos.DrawCube(position, gizmoScale);
            }
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

#endif
    }
}
