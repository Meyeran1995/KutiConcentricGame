using Meyham.Player;
using UnityEngine;

namespace Meyham.GameMode
{
    public class PlayerBodyPartPool : AnObjectPoolBehaviour
    {
        public PlayerBodyPart GetBodyPart()
        {
            pool.Get(out var part);

            return part.GetComponent<PlayerBodyPart>();
        }
    }
}