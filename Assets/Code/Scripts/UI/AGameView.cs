using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;
        
        
        public virtual void OpenView(int animatorId)
        {
            animator.SetBool(animatorId, true);
        }

        public virtual void CloseView(int animatorId)
        {
            animator.SetBool(animatorId, false);
        }
    }
}