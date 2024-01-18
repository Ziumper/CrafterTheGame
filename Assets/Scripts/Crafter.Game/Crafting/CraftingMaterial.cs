using Crafter.Game.Equipment;
using System;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    [Serializable]
    public class CraftingMaterial
    {
        [field: SerializeField] public int AmountNeeded;
        [field: SerializeField] public EquipmentObject Material { get; set; }
    }
}
