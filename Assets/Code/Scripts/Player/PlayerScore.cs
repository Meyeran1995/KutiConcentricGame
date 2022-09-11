using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerScore : MonoBehaviour
    {
        [ReadOnly, SerializeField] private float score;
        [ReadOnly] public int PlayerNumber;
        
        public void AcquireScore(float gainedScore)
        {
            score += gainedScore;
        }

        public void ResetScore() => score = 0;

        public string GetScore() => Mathf.RoundToInt(score).ToString();
    }
}