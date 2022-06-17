using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/FloatValue")]
    public class FloatValue : ValueSO<float>
    {
        public static float operator +(FloatValue a) => +a.RuntimeValue;

        public static float operator -(FloatValue a) => -a.RuntimeValue;
        
        public static float operator +(FloatValue a, FloatValue b) => a.RuntimeValue + b.RuntimeValue;

        public static float operator -(FloatValue a, FloatValue b) => a.RuntimeValue - b.RuntimeValue;
        
        public static float operator *(FloatValue a, FloatValue b) => a.RuntimeValue * b.RuntimeValue;

        public static float operator /(FloatValue a, FloatValue b) => a.RuntimeValue / b.RuntimeValue;

        public static float operator +(FloatValue a, float b) =>  a.RuntimeValue + b;

        public static float operator -(FloatValue a, float b) =>  a.RuntimeValue - b;
        
        public static float operator *(FloatValue a, float b) => a.RuntimeValue * b;

        public static float operator /(FloatValue a, float b) => a.RuntimeValue / b;
    }
}
