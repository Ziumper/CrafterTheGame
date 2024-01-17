using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    [Serializable]
    public class EquipmentBag : IEquipmentBag
    {
        [Serializable]
        public class EquipmentListItem 
        {
            public EquipmentObject Equipment;
            public List<GameObject> GameObjects = new List<GameObject>();
        }

        [SerializeField]
        private List<EquipmentListItem> _items = new List<EquipmentListItem>();

        public bool AddToBag(GameObject equipment)
        {
            var behaviour = equipment.GetComponent<EquipmentBehaviour>();
            if(behaviour == null)
            {
                Debug.LogError("Can't add to bag. No equipment behaviour provided for equipment gameObject", equipment);
                return false;
            }

            var objectFromEquipment = behaviour.EquipmentObject;
            if(objectFromEquipment == null)
            {
                Debug.LogError("Can't add to bag. No equipment object provided for equipment behaviour in equipment gameObject", equipment);
                return false;
            }

            var equipmentListItem = _items.FirstOrDefault(item => item.Equipment.Equals(objectFromEquipment));
            bool notConatinsEquipment = equipmentListItem == null;
            if (notConatinsEquipment)
            {
                _items.Add(
                    new EquipmentListItem
                    {
                        Equipment = objectFromEquipment,
                        GameObjects = new List<GameObject>() { equipment }
                    });

                return true;
            }

            equipmentListItem.GameObjects.Add(equipment);

            Debug.Log($"Added equipment object to bag {equipment.name}", equipment);
            return true;
        }

        public bool RemoveFromBag(GameObject equipment)
        {
            Debug.Log($"Removed equipment object from bag {equipment.name}", equipment);
            return true;
        }
    }
}
