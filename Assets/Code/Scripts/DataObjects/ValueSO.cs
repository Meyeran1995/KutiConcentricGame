using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ValueSO<T> : ScriptableObject
    {
        [SerializeField] private T value;
        public T Value => value;

        public static implicit operator T(ValueSO<T> valueSO) => valueSO.value;
    }
}