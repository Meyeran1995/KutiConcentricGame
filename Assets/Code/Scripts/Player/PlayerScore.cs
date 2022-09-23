using System;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerScore : MonoBehaviour, IComparable<PlayerScore>
    {
        [ReadOnly, SerializeField] private float score;
        [ReadOnly] public int PlayerNumber;
        
        public void AcquireScore(float gainedScore)
        {
            score += gainedScore;
        }

        public void ResetScore() => score = 0;

        public string GetScoreText() => Mathf.RoundToInt(score).ToString();

        public float GetScore() => Mathf.RoundToInt(score);
        
        public int CompareTo(PlayerScore other)
        {
           return other.PlayerNumber.CompareTo(PlayerNumber);
        }
    }
}