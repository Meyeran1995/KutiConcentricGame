using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerScore : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerController controller;
        
        [Header("Debug")]
        [ReadOnly, SerializeField] private float score;

        public PlayerDesignation Designation => controller.Designation;
        
        public void AcquireScore(float gainedScore)
        {
            score += gainedScore;
        }

        public void ResetScore() => score = 0;

        public string GetScoreText() => Mathf.RoundToInt(score).ToString();

        public float GetScore() => Mathf.RoundToInt(score);
    }
}