using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ValueSO<T> : ScriptableObject
    {
        [SerializeField] private T baseValue;

        public T BaseValue => baseValue;

        [field: SerializeField, ReadOnly]
        public T RuntimeValue { get; set; }

        private void OnEnable() => RuntimeValue = baseValue;

        public void ResetToBaseValue() => RuntimeValue = baseValue;

        public static implicit operator T(ValueSO<T> valueSO) => valueSO.RuntimeValue;
    }
}