using System.Collections;
using DG.Tweening;
using UnityEngine;
using static DG.Tweening.DOTweenCYInstruction;

namespace Meyham.Items
{
    public class ItemTweeningAnimation : MonoBehaviour
    {
        [SerializeField] private float growTime;
        [SerializeField] private float shrinkTime;

        public IEnumerator TweenShrink()
        {
            var tween = transform.DOScale(Vector3.zero, shrinkTime);
            return new WaitForCompletion(tween);
        }
        
        private void OnEnable()
        {
            transform.DOScale(Vector3.one, growTime);
        }
    }
}
