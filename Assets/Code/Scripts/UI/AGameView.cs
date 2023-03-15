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

        public virtual IEnumerator OpenView()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            return effect.ShowAsync();
        }

        public virtual IEnumerator CloseView()
        {
            yield return effect.HideAsync();
            
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}