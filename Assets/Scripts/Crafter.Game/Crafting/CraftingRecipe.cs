using Crafter.Game.Equipment;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafter/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [Min(0f), SerializeField] private float _chanceOfSuccess;
        public float ChanceOfSuccess { get => _chanceOfSuccess; }
        [field: SerializeField] public List<CraftingMaterial> Materials { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public EquipmentObject EquipmentToCraft { get; private set; }

        private void OnValidate()
        {
            if(_chanceOfSuccess > 1f)
            {
                _chanceOfSuccess = 1f;
            }
        }
    }
}
