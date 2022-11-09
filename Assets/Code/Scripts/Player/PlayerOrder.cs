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


        public void OrderPlayer(int order)
        {
            spriteRenderer.sprite = modelProvider.GetModel(order);
            
            collisionHelper.ModifyCollisionSize(sizeFactor.RuntimeValue, order);            
        }
    }
}