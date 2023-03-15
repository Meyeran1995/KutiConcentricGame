using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Meyham.UI
{
    public class TimelineEffect : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        
        [Header("Animations")]
        [SerializeField] private PlayableAsset show;
        [SerializeField] private PlayableAsset hide;

        [Header("Children")]
        [SerializeField] private TimelineEffect[] childEffects;

        private bool hasChildEffects;

        public IEnumerator ShowAsync()
        {
            HiddenState();

            director.Play();
            director.timeUpdateMode = DirectorUpdateMode.GameTime;

            var waitForDirector = new WaitForDirector(director);
            StartCoroutine(ShowRoutine(waitForDirector));
            
            return waitForDirector;
        }

        public IEnumerator HideAsync()
        {
            IdleState();
            
            director.Play();
            director.timeUpdateMode = DirectorUpdateMode.GameTime;

            var waitForDirector = new WaitForDirector(director);
            StartCoroutine(HideRoutine(waitForDirector));
            
            return waitForDirector;
        }

        private void Awake()
        {
            hasChildEffects = childEffects != null && childEffects.Length > 0;
        }

        private void HiddenState()
        {
            director.playableAsset = show;
            director.time = 0.0;
            director.Evaluate();
            director.Pause();

            if (!hasChildEffects) return;
            
            foreach (var effect in childEffects)
            {
                effect.HiddenState();
            }
        }
        
        private void IdleState()
        {
            director.playableAsset = hide;
            director.time = 0.0;
            director.Evaluate();
            director.Pause();
            
            if (!hasChildEffects) return;
            
            foreach (var effect in childEffects)
            {
                effect.IdleState();
            }
        }

        private IEnumerator ShowRoutine(IEnumerator waitForDirector)
        {
            yield return waitForDirector;
            IdleState();
        }
        
        private IEnumerator HideRoutine(IEnumerator waitForDirector)
        {
            yield return waitForDirector;
            HiddenState();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            director ??= GetComponent<PlayableDirector>();

            if (childEffects != null)
            {
                return;
            }
            
            var effects = GetComponentsInChildren<TimelineEffect>();

            if (effects.Length == 1)
            {
                return;
            }
            
            childEffects = new TimelineEffect[effects.Length - 1];

            for (int i = 1; i < childEffects.Length; i++)
            {
                childEffects[i] = effects[i];
            }
        }
#endif
    }
}
