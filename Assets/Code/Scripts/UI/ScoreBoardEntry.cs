using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        [Header("Names")]
        [SerializeField] private TextMeshProUGUI nameUp;
        [SerializeField] private TextMeshProUGUI nameDown;

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
            nameUp.color = color;
            nameDown.color = color;

            scoreUp.color = color;
            scoreDown.color = color;
        }

#if UNITY_EDITOR
        
        public void SetPlayerName(string playerName)
        {
            nameUp.text = playerName;
            nameDown.text = playerName;
        }
        
#endif
    }
}