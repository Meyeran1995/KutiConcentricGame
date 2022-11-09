using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class PlayerPositionTracker : MonoBehaviour
    {
        [SerializeField] private FloatValue angleGain, collisionThresh;

        private static readonly List<PlayerController> Players = new();
        private static List<PlayerController> pendingMovements, collisions;

        public static int MaxPosition;
        
        public float[] StartingPositions { get; private set; }

        public static void InitializeLists(int playerCount)
        {
            pendingMovements = new List<PlayerController>(playerCount);
            collisions = new List<PlayerController>(playerCount - 1);
        }
        
        public static void Register(PlayerController player)
        {
            Players.Add(player);
        }

        public static void MovePosition(PlayerController player)
        {
            pendingMovements.Add(player);
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

        private void LateUpdate()
        {
            if (pendingMovements == null || pendingMovements.Count == 0) return;
            
            for (int i = pendingMovements.Count - 1; i >= 0; i--)
            {
                MovePlayer(pendingMovements[i]);
            }
            
            pendingMovements.Clear();
        }

        private void MovePlayer(PlayerController player)
        {
            player.ChangeOrder(0);

            foreach (var playerController in GetCollisions(player, player.Movement.LastPosition))
            {
                playerController.DecrementOrder();
            }
            
            foreach (var playerController in GetCollisions(player, player.transform.position))
            {
                playerController.IncrementOrder();
            }
        }

        private IEnumerable<PlayerController> GetCollisions(PlayerController player, Vector3 playerPosition)
        {
            collisions.Clear();
            
            foreach (var playerController in Players)
            {
                if(playerController == player) continue;
                
                float distance = Vector2.Distance(playerPosition, playerController.transform.position);

                if(distance > collisionThresh) continue;
                
                collisions.Add(playerController);
            }

            return collisions;
        }
    }
}