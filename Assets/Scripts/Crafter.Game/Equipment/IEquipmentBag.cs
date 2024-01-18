using System;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBag
    {
        void ToggleEquipmentPanel();
        void AddToBag(GameObject equipment);
        bool RemoveFromBag(GameObject equipment);
        bool ContainsEquipment(EquipmentObject equipment, int amount);
        void DestroyAndRemoveFromBag(EquipmentObject equipment, int amount);
    }
}
