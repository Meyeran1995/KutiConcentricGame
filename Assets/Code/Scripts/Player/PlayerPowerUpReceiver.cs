using System.Collections;
using Meyham.Events;
using Meyham.Items;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerPowerUpReceiver : MonoBehaviour
    {
        [SerializeField] private GenericEventChannelSO<APowerUpEffect> negativePowerUpEvent;
        [SerializeField] private GameObject target;
        
        private APowerUpEffect lastAPowerUpReceived;
        
        private void Start()
        {
            negativePowerUpEvent += OnNegativeEffectCollected;
        }

        public void Receive(APowerUpEffect effect)
        {
            lastAPowerUpReceived = effect;
            
            if (effect.HasSingleTarget)
            {
                effect.Apply(target);
                return;
            }
            
            negativePowerUpEvent.RaiseEvent(effect);
        }

        private void OnNegativeEffectCollected(APowerUpEffect effect)
        {
            if (effect == lastAPowerUpReceived)
            {
                lastAPowerUpReceived = null;
                 return;
            }
            
            ApplyPowerUpEffect(effect);
        }

        private void ApplyPowerUpEffect(APowerUpEffect effect)
        {
            effect.Apply(target);
            StartCoroutine(PowerUpEffectDuration(effect));
        }

        private IEnumerator PowerUpEffectDuration(APowerUpEffect effect)
        {
            float duration = effect.Duration;

            yield return new WaitForSeconds(duration);
            
            effect.Remove(target);
        }
    }
}