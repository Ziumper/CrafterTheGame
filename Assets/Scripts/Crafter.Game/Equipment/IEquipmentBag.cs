using System;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBag
    {
        void ToggleEquipmentPanel();
        void AddToBag(GameObject equipment);
        void RemoveFromBag(GameObject equipment);
    }
}
