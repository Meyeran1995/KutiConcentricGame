using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class PlayerPositionTracker : MonoBehaviour
    {
        [SerializeField] private FloatValue angleGain;

        private static readonly Dictionary<RadialPlayerMovement, PlayerController> Players = new ();

        public static int MaxPosition;
        
        public float[] StartingPositions { get; private set; }

        public static void Register(PlayerController player)
        {
            Players.Add(player.Movement, player);
        }

        public static void MovePosition(RadialPlayerMovement player, int direction)
        {
            var controller = Players[player];
            int lastPosition = controller.PositionIndex;
            int lastOrder = controller.Order;
            int nextPosition = lastPosition + direction;

            if (nextPosition < 0)
            {
                nextPosition = MaxPosition - 1;
            }
            else if (nextPosition == MaxPosition)
            {
                nextPosition = 0;
            }

            controller.ChangeOrder(GetOrder(nextPosition));
            controller.PositionIndex = nextPosition;
            
            DecrementPlayerOrderAtPosition(lastPosition, lastOrder);
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

        private void Start()
        {
            MaxPosition = Mathf.RoundToInt(360f / angleGain.BaseValue);
            
            float currentAngle = 0f;
            StartingPositions = new float[MaxPosition];

            for (int i = 0; i < MaxPosition; i++)
            {
                StartingPositions[i] = currentAngle;
                currentAngle += angleGain;
            }
        }

        private static int GetOrder(int position)
        {
            int order = 0;

            foreach (var playerPosition in Players.Values)
            {
                if(playerPosition.PositionIndex != position) continue;
                order++;
            }

            return order;
        }

        private static void DecrementPlayerOrderAtPosition(int position, int leavingOrder)
        {
            foreach (var playerPosition in Players.Values)
            {
                if(playerPosition.PositionIndex != position) continue;
                
                int order = playerPosition.Order;
                
                if(order < leavingOrder) continue;
                
                order--;

                if (order < 0)
                    order = 0;

                playerPosition.ChangeOrder(order);
            }
        }
    }
}