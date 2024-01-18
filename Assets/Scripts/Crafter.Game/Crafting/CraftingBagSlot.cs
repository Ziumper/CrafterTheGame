using Crafter.Game.Equipment;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Crafting
{
    public class CraftingBagSlot : MonoBehaviour, IEquipmentBagSlot
    {
        public bool IsEmpty => throw new NotImplementedException();

        [field: SerializeField] public UnityEvent<IEquipmentBagSlot> OnSlotClicked { get; private set; }

        public EquipmentObject Equipment => throw new NotImplementedException();

        public int SlotCount => throw new NotImplementedException();

        public void AddOne(EquipmentObject equipmentObject, GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void AddOne(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public List<GameObject> RemoveMany(EquipmentObject equipmentObject, int amount)
        {
            throw new NotImplementedException();
        }

        public GameObject RemoveOne()
        {
            throw new NotImplementedException();
        }
    }
}
