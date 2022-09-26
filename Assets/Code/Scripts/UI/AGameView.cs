using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;

        protected virtual void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public virtual void OpenView(int animatorId)
        {
            animator.SetBool(animatorId, true);
        }

        public virtual void CloseView(int animatorId)
        {
            animator.SetBool(animatorId, false);
        }

        public abstract void SetTextColor(int playerId, Color color);
    }
}