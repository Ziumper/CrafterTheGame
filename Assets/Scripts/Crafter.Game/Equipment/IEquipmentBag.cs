using System;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBag
    {
        UnityEvent<bool> OnPanelToggled { get; }
        void ToggleEquipmentPanel();
        void AddToBag(GameObject equipment);
        bool RemoveFromBag(GameObject equipment);
        bool ContainsEquipment(EquipmentObject equipment, int amount);
        void DestroyAndRemoveFromBag(EquipmentObject equipment, int amount);
    }
}
