using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;

        private static readonly int ViewIsOpenId = Animator.StringToHash("IsOpen");

        protected virtual void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public virtual void OpenView()
        {
            animator.SetBool(ViewIsOpenId, true);
        }

        public virtual void CloseView()
        {
            animator.SetBool(ViewIsOpenId, false);
        }
    }
}