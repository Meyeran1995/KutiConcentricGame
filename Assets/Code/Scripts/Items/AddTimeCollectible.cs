using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Items
{
    public class AddTimeCollectible : ACollectible
    {
        [SerializeField] private FloatValue timeToAdd;
        

        protected override void OnCollect()
        {
            Debug.Log($"You gained {timeToAdd} time");
        }
    }
}
