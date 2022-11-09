using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [SerializeField] private FloatValue sizeFactor;
        [SerializeField] private PlayerModelProvider modelProvider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PlayerCollisionHelper collisionHelper;

        private const float OrderDisplacementAmount = 32f / 100f;
        
        public void OrderPlayer(int order)
        {
            spriteRenderer.sprite = modelProvider.GetModel(order);
            transform.localPosition = new Vector3(-order * OrderDisplacementAmount, 0f, 0f);
            collisionHelper.ModifyCollisionSize(sizeFactor.RuntimeValue, order);            
        }
    }
}