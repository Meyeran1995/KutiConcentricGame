using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject collisionParent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Player References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerScore score;
        [SerializeField] private PlayerOrder playerOrder;

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
            spriteRenderer.color = color;
        }

        public void SetStartingPosition(float angle)
        {
            movement.SnapToStartingAngle(angle);
        }

        private void OnEnable()
        {
            if(!IsActive) return;
            
            movement.enabled = true;
            input.enabled = true;
            playerOrder.enabled = true;
            collisionParent.SetActive(true);
            score.ResetScore();
        }

        private void OnDisable()
        {
            if(!IsActive) return;

            movement.enabled = false;
            input.enabled = false;
            playerOrder.enabled = false;
            collisionParent.SetActive(false);
        }

        private void Awake()
        {
            gameObject.name = Designation.ToString();
        }
    }
}