using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Collision
{
    public static class PlayerCollisionHelper
    {
        private static Transform origin;
        
        private static readonly LayerMaskProvider MaskProvider = new ();
        
        private static readonly Dictionary<Collider, PlayerCollision> ColliderToPlayer = new(6);

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

        public static void Register(Collider playerCollider, PlayerCollision playerCollision)
        {
            ColliderToPlayer.Add(playerCollider, playerCollision);
        }

        public static PlayerCollision GetPlayerByCollider(Collider playerCollider)
        {
            return ColliderToPlayer[playerCollider];
        }
    }
}