using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public abstract class MonoEquipmentBag : MonoBehaviour, IEquipmentBag
    {
        [SerializeField] protected IEquipmentBagSlot[] _slots;
        [SerializeField] protected GameObject _equipmentPanel;

        protected Dictionary<EquipmentObject, IEquipmentBagSlot> _equipmentDictionary;

        public void ToggleEquipmentPanel()
        {
            _equipmentPanel.SetActive(!_equipmentPanel.activeSelf);
        }

        public virtual void AddToBag(GameObject equipment)
        {
            var behaviour = gameObject.GetComponent<EquipmentBehaviour>();
            if (behaviour == null)
            {
                Debug.LogError("Can't add to bag. No equipment behaviour provided for equipment gameObject", gameObject);
                return;
            }

            var objectFromEquipment = behaviour.EquipmentObject;
            if (objectFromEquipment == null)
            {
                Debug.LogError("Can't add to bag. No equipment object provided for equipment behaviour in equipment gameObject", gameObject);
                return;
            }

            if (_equipmentDictionary.TryGetValue(objectFromEquipment, out var bagSlot))
            {
                bagSlot.AddOne(gameObject);
                Debug.Log($"Added equipment object to bag {objectFromEquipment.Name}", gameObject);
                return;
            }

            bagSlot = _slots.FirstOrDefault(_item => _item.IsEmpty);
            if (bagSlot == null)
            {
                Debug.Log($"Can't add {objectFromEquipment.Name} to bag. No empty slots inside bag!");
                return;
            }

            bagSlot.AddOne(objectFromEquipment, gameObject);
            _equipmentDictionary.Add(objectFromEquipment,bagSlot);
            Debug.Log($"Added equipment {objectFromEquipment.Name} to bag", gameObject);
        }

        public virtual bool RemoveFromBag(GameObject equipment) 
        {
            var equipmentBehaviour = gameObject.GetComponent<EquipmentBehaviour>();

            if (equipmentBehaviour == null)
            {
                Debug.LogError("No equipment behaviour provided can't remove from bag", gameObject);
                return false;
            }

            if (equipmentBehaviour.EquipmentObject == null)
            {
                Debug.LogError("Can't remove. No equipment provided in equipment object", gameObject);
                return false;
            }

            Debug.Log($"Removed equipment {equipmentBehaviour.EquipmentObject.Name} from bag", gameObject);
            return true;
        }

        private void Start() { Init(); }

        protected virtual void Init()
        {
            if (_slots.Length <= 0)
            {
                FindSlots();
            }

            foreach (var slot in _slots)
            {
                slot.OnSlotClicked.AddListener(OnBagSlotCliked);
            }
        }

        [ContextMenu("Find Bag Slots")]
        private void FindSlots()
        {
            _slots = _equipmentPanel.GetComponentsInChildren<EquipmentBagSlot>(true);
        }

        public virtual void OnBagSlotCliked(IEquipmentBagSlot slot)
        {
            if (!slot.IsEmpty)
            {
                RemoveFromBag(slot.RemoveOne());
                if(slot.IsEmpty)
                {
                    _equipmentDictionary.Remove(slot.Equipment);
                }
            }
        }

        public bool ContainsEquipment(EquipmentObject equipment, int amount)
        {
            if(_equipmentDictionary.TryGetValue(equipment,out var slot))
            {
                if (slot.SlotCount < amount) return false;

                return true;
            }

            return false;
        }

        public void DestroyAndRemoveFromBag(EquipmentObject equipment, int amount)
        {
            if(_equipmentDictionary.TryGetValue(equipment,out var slot))
            {
                if(slot.SlotCount < amount)
                {
                    Debug.LogError($"Not enoguht items in equipment bag for {equipment.Name}. Required: {amount}, has: {slot.SlotCount}", gameObject);
                    return;
                }

                var gameObjects = slot.RemoveMany(equipment, amount);
                foreach(var gameObject in gameObjects)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
