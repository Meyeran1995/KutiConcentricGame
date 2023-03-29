namespace Meyham.Collision
{
    public class LayerMaskProvider
    {
        private int[] lowerMasks;
        private int[] upperMasks;
        
        private const int layer_offset = 6;

        public LayerMaskProvider()
        {
            lowerMasks = new int[5];
            upperMasks = new int[5];
            
            lowerMasks[0] = 1 << layer_offset;
            for (int i = 1; i < lowerMasks.Length; i++)
            {
                int targetLayer = 1 << i + layer_offset;
                lowerMasks[i] = lowerMasks[i - 1] | targetLayer;
            }

            for (int i = 0; i < lowerMasks.Length; i++)
            {
                upperMasks[i] = ~lowerMasks[i];
            }
        }

        public int GetMaskForLowerOrders(int playerOrder)
        {
            return lowerMasks[playerOrder - 1];
        }
        
        public int GetMaskForSameOrder(int playerOrder)
        {
            return 1 << GetLayer(playerOrder);
        }
        
        public int GetMaskForHigherOrders(int playerOrder)
        {
            return upperMasks[playerOrder];
        }

        public int GetLayer(int playerOrder)
        {
            return playerOrder + layer_offset;
        }
    }
}