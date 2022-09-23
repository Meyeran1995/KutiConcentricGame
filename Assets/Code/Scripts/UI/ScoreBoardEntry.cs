using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        private TextMeshProUGUI[] texts;

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
        
        private void Awake()
        {
            texts = new TextMeshProUGUI[4];
            
            var textRoot = transform.GetChild(0);
            texts[0] = textRoot.GetChild(0).GetComponent<TextMeshProUGUI>();
            texts[1] = textRoot.GetChild(1).GetComponent<TextMeshProUGUI>();
            
            textRoot = transform.GetChild(1);
            texts[2] = textRoot.GetChild(0).GetComponent<TextMeshProUGUI>();
            texts[3] = textRoot.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
    }
}