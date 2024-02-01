using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Collision
{
    public static class PlayerCollisionHelper
    {
        private static Transform origin;
        
        private static readonly LayerMaskProvider MaskProvider = new();
        
        private static readonly Dictionary<BodyPart, BodyCollision> BodyPartToCollision = new(18);

        public static Vector3 GetOriginPos()
        {
            origin ??= GameObject.FindGameObjectWithTag("Origin").transform;
            
            return origin.position;
        }

        public static int GetMaskForLowerOrders(int playerOrder)
        {
            return MaskProvider.GetMaskForLowerOrders(playerOrder);
        }
        
        public static int GetMaskForHigherOrders(int playerOrder)
        {
            return MaskProvider.GetMaskForHigherOrders(playerOrder);
        }
        
        public static int GetMaskForSameOrder(int playerOrder)
        {
            return MaskProvider.GetMaskForSameOrder(playerOrder);
        }
        
        public static int GetLayer(int playerOrder)
        {
            return MaskProvider.GetLayer(playerOrder);
        }

        public static void Register(BodyPart bodyPart, BodyCollision bodyCollision)
        {
            BodyPartToCollision.Add(bodyPart, bodyCollision);
        }
        
        public static void DeRegister(BodyPart bodyPart)
        {
            BodyPartToCollision.Remove(bodyPart);
        }
        
        public static BodyCollision GetCollisionByBodyPart(BodyPart bodyPart)
        {
            return BodyPartToCollision[bodyPart];
        }
    }
}