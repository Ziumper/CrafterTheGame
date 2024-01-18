using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public abstract class MonoEquipmentBag : MonoBehaviour, IEquipmentBag
    {
        [SerializeField] protected EquipmentBagSlot[] _slots;
        [SerializeField] protected GameObject _equipmentPanel;

        protected Dictionary<EquipmentObject, EquipmentBagSlot> _equipmentDictionary;

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
                bagSlot.AddToSlot(gameObject);
                Debug.Log($"Added equipment object to bag {objectFromEquipment.Name}", gameObject);
                return;
            }

            bagSlot = _slots.FirstOrDefault(_item => _item.IsEmpty);
            if (bagSlot == null)
            {
                Debug.Log($"Can't add {objectFromEquipment.Name} to bag. No empty slots inside bag!");
                return;
            }

            bagSlot.AddToSlot(objectFromEquipment, gameObject);
            _equipmentDictionary.Add(objectFromEquipment,bagSlot);
            Debug.Log($"Added equipment {objectFromEquipment.Name} to bag", gameObject);
        }

        public virtual void RemoveFromBag(GameObject equipment) { }

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

        private void OnBagSlotCliked(EquipmentBagSlot slot)
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
    }
}
