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

        public IEnumerator ShowAsync()
        {
            if (childEffects != null && childEffects.Length > 0)
            {
                foreach (var effect in childEffects)
                {
                    effect.PrepareShow();
                }
            }

            director.Play(show, DirectorWrapMode.None);

            return new WaitForDirector(director);
        }

        public IEnumerator HideAsync()
        {
            if (childEffects != null && childEffects.Length > 0)
            {
                foreach (var effect in childEffects)
                {
                    effect.PrepareHide();
                }
            }
            
            director.Play(hide, DirectorWrapMode.None);

            return new WaitForDirector(director);
        }

        private void PrepareShow()
        {
            director.playableAsset = show;
            director.time = 0.0;
            director.timeUpdateMode = DirectorUpdateMode.Manual;
            director.Evaluate();
        }
        
        private void PrepareHide()
        {
            director.playableAsset = hide;
            director.time = 0.0;
            director.Evaluate();
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
