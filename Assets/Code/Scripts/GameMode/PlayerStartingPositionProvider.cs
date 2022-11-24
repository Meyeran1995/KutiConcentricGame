using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class PlayerStartingPositionProvider
    {
        private float[] StartingPositions;

        public PlayerStartingPositionProvider(int playerCount)
        {
            float angleGain = Mathf.RoundToInt(360f / playerCount);
            float currentAngle = 0f;
            StartingPositions = new float[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                StartingPositions[i] = currentAngle;
                currentAngle += angleGain;
            }
        }
        
        public void RotateStartingPositions()
        {
            float rotation = Random.Range(10f, 350f);

            for (int i = 0; i < StartingPositions.Length; i++)
            {
                float position = StartingPositions[i];
                position += rotation;
                position %= 360f;
                StartingPositions[i] = position;
            }
        }

        public float GetStartingPosition(int index)
        {
            return StartingPositions[index];
        }
    }
}