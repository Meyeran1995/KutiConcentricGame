namespace Meyham.Collision
{
    public class LayerMaskProvider
    {
        private int[] masks;
        
        private const int layer_offset = 6;

        public LayerMaskProvider()
        {
            masks = new int[5];
            
            masks[0] = 1 << layer_offset;
            for (int i = 1; i < masks.Length; i++)
            {
                int targetLayer = 1 << i + layer_offset;
                masks[i] = masks[i - 1] | targetLayer;
            }
        }

        public int GetMask(int playerOrder)
        {
            return masks[playerOrder - 1];
        }

        public int GetLayer(int playerOrder)
        {
            return playerOrder + layer_offset;
        }
    }
}