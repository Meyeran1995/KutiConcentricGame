using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [Header("Animation")]
        [SerializeField] private TimelineEffect effect;

        protected virtual void Awake()
        {
            content.SetActive(false);
        }

        public virtual IEnumerator OpenView()
        {
            content.SetActive(true);
            return effect.ShowAsync();
        }

        public virtual IEnumerator CloseView()
        {
            var waitObject = effect.HideAsync();
            StartCoroutine(CloseRoutine(waitObject));
            return waitObject;
        }
        
        public abstract void Clean();

        private IEnumerator CloseRoutine(IEnumerator waitForDirector)
        {
            yield return waitForDirector;
            content.SetActive(false);
        }
    }
}