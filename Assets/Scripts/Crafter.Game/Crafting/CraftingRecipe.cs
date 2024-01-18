using Crafter.Game.Equipment;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafter/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [field: SerializeField] public List<CraftingMaterial> Materials { get; set; }
        [field: SerializeField] public float ChanceOfSuccess { get; set; }
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public EquipmentObject EquipmentToCraft { get; set; } 
    }
}
