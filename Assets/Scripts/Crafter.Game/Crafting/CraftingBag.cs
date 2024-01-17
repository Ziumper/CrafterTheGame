using Crafter.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    public class CraftingBag : MonoBehaviour, IEquipmentBag, ICraftingBag
    {
        [SerializeField] private List<CraftingRecipe> _recipes;
        [SerializeField] private EquipmentBagSlot[] _slots;
        [SerializeField] private GameObject _craftingPanel;
        [SerializeField] private EquipmentBag _equipmentBag;

        private void Start()
        {
            if (_slots.Length <= 0) { FindSlots(); }

            foreach(var slot in _slots) 
            {
                slot.OnSlotClicked.AddListener(OnBagSlotCliked);
            }

            _equipmentBag = gameObject.GetComponent<EquipmentBag>();
        }

        private void OnBagSlotCliked(EquipmentBagSlot slot)
        {
            if (!slot.IsEmpty)
            {
                RemoveFromBag(slot.RemoveOne());
            }
        }

        public void AddToBag(GameObject equipment)
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

            var bagSlot = _slots.FirstOrDefault(item => item.ContainsEquipment(objectFromEquipment));
            bool notConatinsEquipment = bagSlot == null;
            if (notConatinsEquipment)
            {
                bagSlot = _slots.FirstOrDefault(_item => _item.IsEmpty);
                if (bagSlot == null)
                {
                    Debug.Log($"Can't add {objectFromEquipment.Name} to bag. No empty slots inside bag!");
                    return;
                }

                bagSlot.AddToSlot(objectFromEquipment, gameObject);
                Debug.Log($"Added equipment {objectFromEquipment.Name} to bag", gameObject);
                return;
            }

            bagSlot.AddToSlot(gameObject);
            Debug.Log($"Added equipment object to bag {objectFromEquipment.Name}", gameObject);
        }

        public void Craft()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFromBag(GameObject equipment)
        {
            _equipmentBag.AddToBag(equipment);
        }

        [ContextMenu("Find Bag Slots")]
        private void FindSlots()
        {
            _slots = _craftingPanel.GetComponentsInChildren<EquipmentBagSlot>(true);
        }
    }
}
