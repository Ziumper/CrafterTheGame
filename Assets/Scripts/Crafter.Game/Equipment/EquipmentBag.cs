using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public class EquipmentBag : MonoBehaviour, IEquipmentBag
    {
        [SerializeField] private EquipmentBagSlot[] _equipmentSlots;
        [SerializeField] private GameObject _equipmentPanel;

        private void Start()
        {
            if (_equipmentSlots.Length <= 0)
            {
                FindSlots();
            }

            foreach(var slot in _equipmentSlots)
            {
                slot.OnSlotClicked.AddListener(OnBagSlotCliked);
            }
        }

        public void ToggleEquipmentPanel()
        {
            _equipmentPanel.SetActive(!_equipmentPanel.activeSelf);
        }

        private void OnBagSlotCliked(EquipmentBagSlot slot)
        {
            if(!slot.IsEmpty)
            {
                RemoveFromBag(slot.RemoveOne());
            }
        }

        public void AddToBag(GameObject gameObject)
        {
            var behaviour = gameObject.GetComponent<EquipmentBehaviour>();
            if(behaviour == null)
            {
                Debug.LogError("Can't add to bag. No equipment behaviour provided for equipment gameObject", gameObject);
                return;
            }

            var objectFromEquipment = behaviour.EquipmentObject;
            if(objectFromEquipment == null)
            {
                Debug.LogError("Can't add to bag. No equipment object provided for equipment behaviour in equipment gameObject", gameObject);
                return;
            }

            var bagSlot = _equipmentSlots.FirstOrDefault(item => item.ContainsEquipment(objectFromEquipment));
            bool notConatinsEquipment = bagSlot == null;
            if (notConatinsEquipment)
            {
                bagSlot = _equipmentSlots.FirstOrDefault(_item => _item.IsEmpty);
                if(bagSlot == null)
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

        public void RemoveFromBag(GameObject gameObject)
        {
            var equipmentBehaviour = gameObject.GetComponent<EquipmentBehaviour>();
            
            if(equipmentBehaviour == null)
            {
                Debug.LogError("No equipment behaviour provided can't remove from bag",gameObject);
                return;
            }

            if(equipmentBehaviour.EquipmentObject == null)
            {
                Debug.LogError("Can't remove. No equipment provided in equipment object", gameObject);
                return;
            }

            gameObject.transform.position = this.gameObject.transform.position;
            gameObject.SetActive(true);

            Debug.Log($"Removed equipment {equipmentBehaviour.EquipmentObject.Name} from bag", gameObject);
        }

        [ContextMenu("Find Bag Slots")]
        private void FindSlots()
        {
            _equipmentSlots = GetComponentsInChildren<EquipmentBagSlot>(true);
        }
    }
}
