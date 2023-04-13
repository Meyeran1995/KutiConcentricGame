using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Vector2Parameter")]
    public class Vector2Parameter : ParameterSO<Vector2>
    {
        public float Magnitude => RuntimeValue.magnitude;
        
        public static Vector2 operator -(Vector2Parameter a) => -a.RuntimeValue;

        public static Vector2 operator +(Vector2Parameter a, Vector2Parameter b) => a.RuntimeValue + b.RuntimeValue;

        public static Vector2 operator -(Vector2Parameter a, Vector2Parameter b) => a.RuntimeValue - b.RuntimeValue;
        
        public static Vector2 operator *(Vector2Parameter a, Vector2Parameter b) => a.RuntimeValue * b.RuntimeValue;

        public static Vector2 operator /(Vector2Parameter a, Vector2Parameter b) => a.RuntimeValue / b.RuntimeValue;

        public static Vector2 operator *(Vector2Parameter a, float b) => a.RuntimeValue * b;

        public static Vector2 operator /(Vector2Parameter a, float b) => a.RuntimeValue / b;
        
        public static implicit operator Vector3(Vector2Parameter vec) => vec.RuntimeValue;
    }
}
