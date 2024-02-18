using System.Collections;
using UnityEngine;

namespace Meyham.UI
{
    public abstract class AGameView : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [Header("Animation")]
        [SerializeField] private TimelineEffect effect;

        protected virtual void Awake()
        {
            canvas.enabled = false;
        }

        public virtual IEnumerator OpenView()
        {
            canvas.enabled = true;
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
            canvas.enabled = false;
        }
    }
}