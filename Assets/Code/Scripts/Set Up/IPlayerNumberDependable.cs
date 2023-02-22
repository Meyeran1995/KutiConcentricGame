namespace Meyham.Set_Up
{
    public interface IPlayerNumberDependable
    {
        public void OnPlayerJoined(int playerNumber);

        public void OnPlayerLeft(int playerNumber);
    }
}