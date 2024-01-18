using UnityEngine;

namespace Crafter.Game.Equipment
{
    public class EquipmentBag : MonoEquipmentBag
    {
        [Tooltip("How far removed object should, be next to player"), SerializeField] private float _throwRadius = 2f;
        [SerializeField] EquipmentBagSlot[] _equipmentBagSlots;

        protected override void Init()
        {
            if(_equipmentBagSlots.Length >= 0)
            {
                _slots = _equipmentBagSlots;
            }
            
            base.Init();
        }

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

        [ContextMenu("Find slots")]
        protected override void FindSlots()
        {
            _equipmentBagSlots = _equipmentPanel.GetComponentsInChildren<EquipmentBagSlot>();
        }
    }
}
