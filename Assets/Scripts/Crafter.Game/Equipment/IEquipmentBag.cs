using UnityEngine;

namespace Crafter.Game.Equipment
{
    public interface IEquipmentBag
    {
        bool AddToBag(GameObject equipment);
        bool RemoveFromBag(GameObject equipment);
    }
}
