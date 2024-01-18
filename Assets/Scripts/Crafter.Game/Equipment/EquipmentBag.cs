using UnityEngine;

namespace Crafter.Game.Equipment
{
    public class EquipmentBag : MonoEquipmentBag
    {
        [Tooltip("How far removed object should, be next to player"), SerializeField] private float _throwRadius = 2f;

        public override bool RemoveFromBag(GameObject gameObject)
        {
            if(base.RemoveFromBag(gameObject))
            {
                gameObject.RotateRandomAround(this.gameObject, _throwRadius);
                gameObject.SetActive(true);
                return true;
            }

            return false;
        }
    }
}
