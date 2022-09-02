using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/StatObjects/ItemMovementStats")]
    public class ItemMovementStatsSO : ScriptableObject
    {
        // Speed
        [field: Header("Speed"), SerializeField] public float Speed { get; private set; }

        // Rotation
        [field: Header("Rotation"), SerializeField] public bool UseGlobalAxis { get; private set; }
        [field: SerializeField] public bool ClockwiseRotation { get; private set; }
        [field: SerializeField, Range(0f, 360f)] public float RotationGain { get; private set; }
        [field: SerializeField, Range(0, 360)] public int MaxAngle { get; private set; }

        public float GetStartingAngle() => Random.Range(0f, 360f);

        // Switch
        [field: Header("Switch Behaviour"), SerializeField, Min(0)] public int Switch { get; private set; }
        [field: SerializeField, Min(1)] public int TimesToSwitch { get; private set; }
        [field: SerializeField, Range(-360f, 360f)] public float RotationGainModifier { get; private set; }
        [field: SerializeField] public float SpeedModifier { get; private set; }
        [field: SerializeField] public bool AxisSwitch { get; private set; }
        [field: SerializeField] public bool RotationDirectionSwitch { get; private set; }
    }
}
