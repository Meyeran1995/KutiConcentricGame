using System;
using System.Collections;
using System.Collections.Generic;
using Meyham.Animation;
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

        public const int MAX_NUMBER_OF_BODY_PARTS = 25;
        
        [Header("Parameters")]
        [SerializeField] private FloatParameter radius;
        [SerializeField] private FloatParameter anglePerBodyPart;
        [SerializeField] private FloatParameter padding;
        
        [Header("References")]
        [SerializeField] private RadialPlayerMovement playerMovement;
        [SerializeField] private GenericEventChannelSO<int> playerDestroyed;
        [SerializeField] private GenericEventChannelSO<int> bodyPartCollectedEventChannel;

        [ReadOnly, SerializeField] private bool headIsFront;
        [ReadOnly, SerializeField] private int hydraIndex;
        [ReadOnly, SerializeField] private PlayerDesignation designation;

        private bool initialized;
        
        private LinkedList<BodyPart> playerBodyParts = new();

        private static PlayerBodyPartPool bodyPartPool;

        private Dictionary<BodyPart, AddBodyCollectionAnimationHandle> collectionAnimationHandleByPart;

        public event Action<BodyPart> BodyPartAcquired;
        
        public event Action<BodyPart> BodyPartLost;

        public int Count => playerBodyParts.Count;

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

        public bool CanAcquireBodyParts()
        {
            return enabled && playerBodyParts.Count < MAX_NUMBER_OF_BODY_PARTS;
        }
        
        public void AcquireBodyPartAnimated(ATweenBasedAnimation animationHandle)
        {
            var incomingPart = AcquireBodyPart();
            incomingPart.Hide();
            
            bodyPartCollectedEventChannel.RaiseEvent((int)designation);

            StartCoroutine(WaitForCollectionAnimation((AddBodyCollectionAnimationHandle)animationHandle, incomingPart));
        }

        public void LoseBodyPartAnimated(ATweenBasedAnimation animationHandle)
        {
            var bodyPart = LoseBodySegment();
            var destructionAnimation = (DestroyBodyCollectionHandle)animationHandle;
            
            if (collectionAnimationHandleByPart.TryGetValue(bodyPart, out var addBodyCollectionAnimationHandle))
            {
                addBodyCollectionAnimationHandle.ReleaseBodyAfterPlaying = true;
                animationHandle.Cancel();
                return;
            }
            
            var destructionOrigin = destructionAnimation.Origin;

            if (bodyPart == destructionOrigin)
            {
                StartCoroutine(WaitForDestructionAnimation(destructionAnimation, bodyPart));
                return;
            }
            
            if (headIsFront)
            {
                var originIndex = 0;
                var node = playerBodyParts.First;
                
                for (; node != null; node = node.Next, originIndex++)
                {
                    if (destructionOrigin == node.Value) break;
                }
                
                if (node == null)
                {
                    animationHandle.Cancel();
                    ReleaseBodyPart(bodyPart);
                    return;
                }

                for (int i = originIndex; i < playerBodyParts.Count; i++)
                {
                    node = node.Next;

                    if (node == null)
                    {
                        break;
                    }
                    
                    destructionAnimation.AddBodyPartToAnimation(node.Value.GetTweenAnimation());
                }
                
                StartCoroutine(WaitForDestructionAnimation(destructionAnimation, bodyPart));
                return;
            }
            
            var buffer = new List<BodyPart>();

            foreach (var part in playerBodyParts)
            {
                buffer.Add(part);
                    
                if(part == destructionOrigin) break;
            }

            buffer.Reverse();

            foreach (var part in buffer)
            {
                destructionAnimation.AddBodyPartToAnimation(part.GetTweenAnimation());
            }
            
            StartCoroutine(WaitForDestructionAnimation(destructionAnimation, bodyPart));
        }

        private IEnumerator WaitForDestructionAnimation(DestroyBodyCollectionHandle animationHandle, BodyPart bodyPart)
        {
            animationHandle.Play();
            
            yield return animationHandle;

            ReleaseBodyPart(bodyPart);
        }
        
        private IEnumerator WaitForCollectionAnimation(AddBodyCollectionAnimationHandle animationHandle, BodyPart incomingPart)
        {
            animationHandle.Play(incomingPart.transform);
            
            collectionAnimationHandleByPart.Add(incomingPart, animationHandle);
            
            yield return animationHandle;

            collectionAnimationHandleByPart.Remove(incomingPart);
            incomingPart.Show();
            
            if (!animationHandle.ReleaseBodyAfterPlaying) yield break;
            
            ReleaseBodyPart(incomingPart);
        }

        public void IgnoreRaycast()
        {
            foreach (var bodyPart in playerBodyParts)
            {
                bodyPart.IgnoreRaycast();
            }
        }
        
        public void AllowRaycast()
        {
            foreach (var bodyPart in playerBodyParts)
            {
                bodyPart.AllowRaycast();
            }
        }
        
        private void Awake()
        {
            collectionAnimationHandleByPart = new Dictionary<BodyPart, AddBodyCollectionAnimationHandle>(2);
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
                    BodyPartAcquired?.Invoke(bodyPart);
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

        private BodyPart AcquireBodyPart()
        {
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

            return incomingPart;
        }
        
        private BodyPart LoseBodySegment()
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

            BodyPartLost?.Invoke(bodyPart);

            return bodyPart;
        }

        private void ReleaseBodyPart(BodyPart bodyPart)
        {
            bodyPartPool.ReleaseBodyPart(bodyPart.gameObject);
            
            if (transform.childCount > 0) return;
            
            playerDestroyed.RaiseEvent((int)designation);
        }
        
        private void AlignBodyPart(int index, BodyPart bodyPart)
        {
            var transformSelf = transform;
            var angle = (anglePerBodyPart + padding) * index + playerMovement.CirclePosition;

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
            var index = headIsFront ? playerBodyParts.Count - 1 : MAX_NUMBER_OF_BODY_PARTS;
            
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
        private float lastPaddingAndWidth;
        
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused) return;
            
            for (int i = 0; i < MAX_NUMBER_OF_BODY_PARTS; i++)
            {
                var angle = i * (anglePerBodyPart + padding) + playerMovement.CirclePosition;
                var position = GetCirclePoint(angle);
                Gizmos.DrawCube(position, gizmoScale);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!EditorApplication.isPlaying || !EditorApplication.isPaused) return;
            
            var positionOffset = new Vector3(0f, radius);
                
            for (int i = 0; i < MAX_NUMBER_OF_BODY_PARTS; i++)
            {
                var angle = i * anglePerBodyPart;
                var position = GetCirclePoint(angle) + positionOffset;
                Gizmos.DrawCube(transform.worldToLocalMatrix * position, gizmoScale);
            }
        }

        private void OnApplicationQuit()
        {
            initialized = false;
        }
        
        private void Update()
        {
            var currentPaddingAndWidth = padding - anglePerBodyPart;
            
            if(Mathf.Abs(lastPaddingAndWidth - currentPaddingAndWidth) < 0.01f)
            {
                lastPaddingAndWidth = currentPaddingAndWidth;
                return;
            }

            int i = 0;

            foreach (var bodyPart in playerBodyParts)
            {
                AlignBodyPart(i, bodyPart);
                i++;
            }
            
            lastPaddingAndWidth = currentPaddingAndWidth;
        }

#endif
    }
}
