using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.UI
{
    public class MainMenuView : AGameView, IPlayerNumberDependable
    {
        [Header("Players")]
        [SerializeField] private Toggle[] playerToggles;

        public void OnPlayerJoined(int playerNumber)
        {
            playerToggles[playerNumber].ToggleImage();
        }

        public void OnPlayerLeft(int playerNumber)
        {
            playerToggles[playerNumber].ToggleImage();
        }

        public override void Clean()
        {
            playerToggles = null;
        }
    }
}