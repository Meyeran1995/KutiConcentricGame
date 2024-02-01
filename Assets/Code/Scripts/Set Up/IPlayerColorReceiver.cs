using UnityEngine;

namespace Meyham.Set_Up
{
    public interface IPlayerColorReceiver
    {
        public void SetColor(int playerId, Color color);
    }
}