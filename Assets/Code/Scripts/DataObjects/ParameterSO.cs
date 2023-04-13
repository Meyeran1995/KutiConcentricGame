using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ParameterSO<T> : ScriptableObject
    {
        [SerializeField] private T baseValue;
        
        public T BaseValue => baseValue;

        [field: SerializeField]
        public T RuntimeValue { get; set; }

        private void OnEnable() => RuntimeValue = baseValue;

        public static implicit operator T(ParameterSO<T> parameterSo) => parameterSo.RuntimeValue;

#if UNITY_EDITOR

        [SerializeField] private bool updateBaseValue;

        private void OnValidate()
        {
            if(!updateBaseValue) return;

            baseValue = RuntimeValue;
        }

#endif
    }
}