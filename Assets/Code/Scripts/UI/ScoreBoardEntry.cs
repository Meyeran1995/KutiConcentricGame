using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] texts;

        public void SetScore(string scoreText)
        {
            texts[1].text = scoreText;
            texts[3].text = scoreText;
        }

        public void SetPlayerName(int playerNumber)
        {
            playerNumber++;
            texts[0].text = $"Spieler {playerNumber}";
            texts[2].text = $"Spieler {playerNumber}";
        }

        public void SetEntryColor(Color color)
        {
            foreach (var text in texts)
            {
                text.color = color;
            }
        }
    }
}