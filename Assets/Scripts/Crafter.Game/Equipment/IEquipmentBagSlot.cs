using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBagSlot
    {
        public UnityEvent<IEquipmentBagSlot> OnSlotClicked { get; }
        public bool IsEmpty { get; }
        public EquipmentObject Equipment { get; } 
        public int SlotCount { get; }
        public void AddOne(EquipmentObject equipmentObject, GameObject gameObject);
        public void AddOne(GameObject gameObject);
        public GameObject RemoveOne();
        public List<GameObject> RemoveMany(EquipmentObject equipmentObject, int amount);


    }
}
