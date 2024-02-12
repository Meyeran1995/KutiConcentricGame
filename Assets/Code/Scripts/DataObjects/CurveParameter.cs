using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/CurveParameter")]
    public class CurveParameter : ParameterSO<AnimationCurve>
    {
        public int Length => RuntimeValue.length;
        
        public float Evaluate(float timeStamp)
        {
            return RuntimeValue.Evaluate(timeStamp);
        }

        public Keyframe GetKeyframe(int index)
        {
            return RuntimeValue[index];
        }
    }
}
