using Meyham.DataObjects;
using Meyham.Player.Bodies;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class CutSceneBodySimulator : MonoBehaviour, IPlayerColorReceiver
    {
        [Header("Parameter")]
        [SerializeField] private FloatParameter radius;

        private DummyBodyPart[] dummyBodyParts;

        public void SetColor(int playerId, Color color)
        {
            for (int i = 0; i < dummyBodyParts.Length; i++)
            {
                var bodyPart = dummyBodyParts[i];
                bodyPart.SetColor(color);
                AlignBodyPart(i, bodyPart.transform);
            }

            dummyBodyParts = null;
        }

        private void Awake()
        {
            dummyBodyParts = GetComponentsInChildren<DummyBodyPart>(true);
        }

        private void AlignBodyPart(int index, Transform bodyPart)
        {
            var transformSelf = transform;
            var angle = PlayerBody.ANGLE_PER_BODY_PART * index;

            if (angle > 360f)
            {
                angle -= 360f;
            }
            
            var circlePos = GetCirclePoint(angle);
            circlePos = transformSelf.worldToLocalMatrix * circlePos;
            circlePos.y += radius;
            
            var localRotation = Quaternion.Inverse(transformSelf.rotation);
            localRotation *= Quaternion.AngleAxis(angle + 90f, Vector3.forward);

            bodyPart.parent = transformSelf;
            bodyPart.SetLocalPositionAndRotation(circlePos, localRotation);
        }
        
        private Vector3 GetCirclePoint(float currentAngle)
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);
        
            return new Vector3(x, y, 0f);
        }
    }
}
