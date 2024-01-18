using Crafter.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    public class CraftingBag : MonoEquipmentBag, ICraftingBag
    {
        [SerializeField] private List<CraftingRecipe> _recipes;
        [SerializeField] private EquipmentBag _equipmentBag;

        protected override void Init()
        {
            base.Init();

            _equipmentBag = gameObject.GetComponent<EquipmentBag>();
        }

        public void Craft()
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveFromBag(GameObject equipment)
        {
            _equipmentBag.AddToBag(equipment);
        }


    }
}
