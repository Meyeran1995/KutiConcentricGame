using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionHelper : MonoBehaviour
    {
        private static Transform origin;
        
        private static LayerMaskProvider maskProvider;
        
        private static readonly Dictionary<Collider, PlayerCollision> ColliderToPlayer = new(6);

        public static Vector3 GetOriginPos()
        {
            return origin.position;
        }

        public static int GetMaskForLowerOrders(int playerOrder)
        {
            return maskProvider.GetMaskForLowerOrders(playerOrder);
        }
        
        public static int GetMaskForHigherOrders(int playerOrder)
        {
            return maskProvider.GetMaskForHigherOrders(playerOrder);
        }
        
        public static int GetMaskForSameOrder(int playerOrder)
        {
            return maskProvider.GetMaskForSameOrder(playerOrder);
        }
        
        public static int GetLayer(int playerOrder)
        {
            return maskProvider.GetLayer(playerOrder);
        }

        public static void Register(Collider playerCollider, PlayerCollision playerCollision)
        {
            ColliderToPlayer.Add(playerCollider, playerCollision);
        }

        public static PlayerCollision GetPlayerByCollider(Collider playerCollider)
        {
            return ColliderToPlayer[playerCollider];
        }
        
        private void Awake()
        {
            maskProvider = new LayerMaskProvider();
            origin = GameObject.FindGameObjectWithTag("Origin").transform;
        }
    }
}