using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject collisionParent;
        
        [Header("Player References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerOrder playerOrder;
        [SerializeField] private PlayerBody playerBody;

        [field: Header("Properties"), SerializeField] 
        public PlayerDesignation Designation { get; private set; }

        [field: SerializeField, ReadOnly] 
        public bool IsActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
        }
        
        public void Deactivate()
        {
            IsActive = false;
        }
        
        public void SetPlayerColor(Color color)
        {
        }

        public void SetStartingPosition(float angle)
        {
            movement.SnapToStartingAngle(angle);
        }

        public float GetCurrentCirclePosition()
        {
            return movement.CirclePosition;
        }

        public void ShowPlayer()
        {
        }
        
        public void HidePlayer()
        {
        }

        private void OnEnable()
        {
            if(!IsActive) return;
            
            movement.enabled = true;
            input.enabled = true;
            playerOrder.enabled = true;
            playerBody.enabled = true;
            
            collisionParent.SetActive(true);
        }

        private void OnDisable()
        {
            if(!IsActive) return;

            movement.enabled = false;
            input.enabled = false;
            playerOrder.enabled = false;
            playerBody.enabled = false;
            
            collisionParent.SetActive(false);
        }

        private void Awake()
        {
            enabled = false;
            movement.enabled = false;
            input.enabled = false;
            playerOrder.enabled = false;
            collisionParent.SetActive(false);
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            gameObject.name = $"Player {Designation}";
        }
        
#endif
    }
}