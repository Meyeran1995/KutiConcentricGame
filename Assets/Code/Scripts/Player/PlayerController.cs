using Meyham.Collision;
using Meyham.EditorHelpers;
using Meyham.Player.Bodies;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerBody playerBody;
        [SerializeField] private PlayerCollision playerCollision;
        
        [field: Header("Properties"), SerializeField] 
        public PlayerDesignation Designation { get; private set; }

        [field: SerializeField, ReadOnly] 
        public bool IsActive { get; private set; }

        private IPlayerColorReceiver playerColor;

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
            playerColor.SetColor((int)Designation, color);
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
            foreach (var bodyPart in playerBody.GetBodyParts())
            {
                bodyPart.Show();
            }
        }
        
        public void HidePlayer()
        {
            foreach (var bodyPart in playerBody.GetBodyParts())
            {
                bodyPart.Hide();
            }
        }

        private void Awake()
        {
            enabled = false;
            movement.enabled = false;
            input.enabled = false;
            
            playerColor = GetComponentInChildren<IPlayerColorReceiver>();

            var velocityCalculator = GetComponent<PlayerVelocityCalculator>();
            playerBody.BodyPartAcquired += velocityCalculator.OnBodyPartAcquired;
            playerBody.BodyPartLost += velocityCalculator.OnBodyPartLost;
        }
        
        private void OnEnable()
        {
            if(!IsActive) return;
            
            movement.enabled = true;
            input.enabled = true;
            playerBody.enabled = true;
            playerCollision.enabled = true;
        }

        private void OnDisable()
        {
            if(!IsActive) return;

            movement.enabled = false;
            input.enabled = false;
            playerBody.enabled = false;
            playerCollision.enabled = false;
        }
    }
}