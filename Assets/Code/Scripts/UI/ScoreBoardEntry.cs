using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreBoardEntry : MonoBehaviour
    {
        [Header("Scores")]
        [SerializeField] private TextMeshProUGUI placementUp;
        [SerializeField] private TextMeshProUGUI placementDown;

        private const string place = ". Platz";
        
        public void SetPlacement(int placement)
        {
            placementUp.text = $"{placement}{place}";
            placementDown.text = $"{placement}{place}";
        }
        
        public void SetEntryColor(Color color)
        {
            placementUp.color = color;
            placementDown.color = color;
        }
    }
}