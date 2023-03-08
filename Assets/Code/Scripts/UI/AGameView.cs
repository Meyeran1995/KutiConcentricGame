using System.Collections;
using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private TimelineEffect effect;

        protected virtual void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public virtual void OpenView()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _ = effect.ShowAsync();
        }

        public virtual void CloseView()
        {
            StartCoroutine(CloseRoutine());
        }

        private IEnumerator CloseRoutine()
        {
            yield return effect.HideAsync();
            
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}