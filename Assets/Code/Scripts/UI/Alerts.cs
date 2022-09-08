using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class Alerts : MonoBehaviour
    {
        private static TextMeshProUGUI upperAlertTxt, lowerAlertTxt;

        public static void SendAlert(string alertText)
        {
            upperAlertTxt.text = alertText;
            lowerAlertTxt.text = alertText;
        }

        public static void ClearAlert()
        {
            upperAlertTxt.text = "";
            lowerAlertTxt.text = "";
        }
        
        private void Awake()
        {
            var alertTexts = GetComponentsInChildren<TextMeshProUGUI>();

            lowerAlertTxt = alertTexts[0];
            upperAlertTxt = alertTexts[1];
        }
    }
}