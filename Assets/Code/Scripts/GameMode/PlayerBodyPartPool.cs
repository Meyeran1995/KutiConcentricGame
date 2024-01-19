using Meyham.Player;
using UnityEngine;

namespace Meyham.GameMode
{
    public class PlayerBodyPartPool : AnObjectPoolBehaviour
    {
        protected override void OnReturnedToPool(GameObject item)
        {
            base.OnReturnedToPool(item);
            item.transform.parent = null;
        }

        public PlayerBodyPart GetBodyPart()
        {
            pool.Get(out var part);

            return part.GetComponent<PlayerBodyPart>();
        }
        
        public void ReleaseBodyPart(GameObject bodyPart)
        {
            pool.Release(bodyPart);
        }
    }
}