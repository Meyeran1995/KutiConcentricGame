using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/FloatParameter")]
    public class FloatParameter : ParameterSO<float>
    {
        public static float operator -(FloatParameter a) => -a.RuntimeValue;
        
        public static float operator +(FloatParameter a, FloatParameter b) => a.RuntimeValue + b.RuntimeValue;

        public static float operator -(FloatParameter a, FloatParameter b) => a.RuntimeValue - b.RuntimeValue;
        
        public static float operator *(FloatParameter a, FloatParameter b) => a.RuntimeValue * b.RuntimeValue;

        public static float operator /(FloatParameter a, FloatParameter b) => a.RuntimeValue / b.RuntimeValue;

        public static float operator +(FloatParameter a, float b) =>  a.RuntimeValue + b;

        public static float operator -(FloatParameter a, float b) =>  a.RuntimeValue - b;
        
        public static float operator *(FloatParameter a, float b) => a.RuntimeValue * b;

        public static float operator /(FloatParameter a, float b) => a.RuntimeValue / b;
    }
}
