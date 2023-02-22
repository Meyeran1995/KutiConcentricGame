using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class PlayerStartingPositionProvider
    {
        private float[] startingPositions;

        public PlayerStartingPositionProvider(int playerCount)
        {
            float angleGain = Mathf.RoundToInt(360f / playerCount);
            float currentAngle = 0f;
            startingPositions = new float[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                startingPositions[i] = currentAngle;
                currentAngle += angleGain;
            }
        }
        
        public void RotateStartingPositions()
        {
            float rotation = Random.Range(10f, 350f);

            for (int i = 0; i < startingPositions.Length; i++)
            {
                float position = startingPositions[i];
                position += rotation;
                position %= 360f;
                startingPositions[i] = position;
            }
        }

        public float GetStartingPosition(int index)
        {
            return startingPositions[index];
        }
    }
}