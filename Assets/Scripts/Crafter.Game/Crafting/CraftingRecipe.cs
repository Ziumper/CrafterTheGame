using System.Collections.Generic;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafter/Equipment")]
    public class CraftingRecipe : ScriptableObject
    {
        [field: SerializeField] public List<CraftingMaterial> Materials { get; set; }
        [field: SerializeField] public float ChanceOfSuccess { get; set; }
        [field: SerializeField] public GameObject ResultPrefab { get; set; }    
    }
}
