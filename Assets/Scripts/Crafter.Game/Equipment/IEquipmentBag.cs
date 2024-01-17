using System;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBag
    {
        void AddToBag(GameObject equipment);
        void RemoveFromBag(GameObject equipment);
    }
}
