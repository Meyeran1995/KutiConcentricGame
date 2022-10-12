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

        private static int maxPosition;
        
        public float[] StartingPositions { get; private set; }

        public static void Register(PlayerController player)
        {
            Players.Add(player.Movement, player);
        }

        public static void MovePosition(RadialPlayerMovement player, int direction)
        {
            int lastPosition = player.PositionIndex;
            int nextPosition = Mathf.Clamp(lastPosition + direction, 0, maxPosition);

            player.PositionIndex = nextPosition;
            Players[player].ChangeOrder(GetOrder(nextPosition));
            
            DecrementPlayerOrderAtPosition(lastPosition);
        }

        public void InitStartingPositions(int playerCount)
        {
            //TODO: Es muss einen weg geben nen index zu bekommen, vlt n fest stratindex pro spieler?
            float positionGain = (float)maxPosition / playerCount * angleGain;
            float currentAngle = 0f;
            StartingPositions = new float[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                StartingPositions[i] = currentAngle;
                currentAngle += positionGain;
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

        public void GetStartingPosition(PlayerController player, int index)
        {
            float startingAngle = StartingPositions[index];

            if (index == 0)
            {
                player.SetStartingPosition(0, startingAngle);
                return;
            }
            
            player.SetStartingPosition(1, startingAngle);
        }

        private void Start()
        {
            maxPosition = Mathf.RoundToInt(360f / angleGain.BaseValue);
        }

        private static int GetOrder(int position)
        {
            int order = 0;

            foreach (var playerPosition in Players.Keys)
            {
                if(playerPosition.PositionIndex != position) continue;
                order++;
            }

            return order;
        }

        private static void DecrementPlayerOrderAtPosition(int position)
        {
            foreach (var playerPosition in Players.Values)
            {
                var movement = playerPosition.Movement;
                if(movement.PositionIndex != position) continue;

                int order = movement.Order - 1;

                if (order < 0)
                    order = 0;

                playerPosition.ChangeOrder(order);
            }
        }
    }
}