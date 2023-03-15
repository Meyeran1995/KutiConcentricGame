using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        [Header("Scores")]
        [SerializeField] private TextMeshProUGUI scoreUp;
        [SerializeField] private TextMeshProUGUI scoreDown;
        
        public void SetScore(string scoreText)
        {
            scoreUp.text = scoreText;
            scoreDown.text = scoreText;
        }
        
        public void SetEntryColor(Color color)
        {
            scoreUp.color = color;
            scoreDown.color = color;
        }
    }
}