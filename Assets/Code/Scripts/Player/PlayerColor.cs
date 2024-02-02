using Meyham.Player.Bodies;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerColor : MonoBehaviour, IPlayerColorReceiver
    {
        private Color activeColor;

        public void SetColor(int playerId, Color color)
        {
            activeColor = color;
            
            var playerBody = GetComponent<PlayerBody>();
            playerBody.BodyPartAcquired += OnBodyPartAcquired;

            foreach (var bodyPart in playerBody.GetBodyParts())
            {
                bodyPart.SetColor(activeColor);
            }
        }

        private void OnBodyPartAcquired(BodyPart bodyPart)
        {
            bodyPart.SetColor(activeColor);
        }
    }
}