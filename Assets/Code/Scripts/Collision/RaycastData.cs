using UnityEngine;

namespace Meyham.Collision
{
    public class RaycastData
    {
        public readonly Vector3 Origin;
        
        public readonly Vector3 Direction;
        
        public readonly float Length;

        public RaycastData(Vector3 origin, Vector3 direction, float length)
        {
            Origin = origin;
            Direction = direction;
            Length = length;
        }
    }
}