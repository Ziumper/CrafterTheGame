using Crafter.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Crafting
{
    public class CraftingBag : MonoEquipmentBag, ICraftingBag
    {
        [SerializeField] private List<CraftingRecipe> _recipes;
        [SerializeField] private IEquipmentBag _equipmentBag;

        private Dictionary<EquipmentObject, CraftingRecipe> _recipiesDictionary;

        public UnityEvent<EquipmentObject> OnCraftingSuccess;
        public UnityEvent<EquipmentObject> OnCraftingFailure;

        protected override void Init()
        {
            base.Init();
            _equipmentBag = gameObject.GetComponent<EquipmentBag>();
            InitializeRecipesDictionary();
        }

        private void InitializeRecipesDictionary()
        {
            _recipiesDictionary = new Dictionary<EquipmentObject, CraftingRecipe>();

            foreach(var recipe in  _recipes)
            {
                AddRecipeToDicitonary(recipe);
            }
        }

        private void AddRecipeToDicitonary(CraftingRecipe recipe)
        {
            _recipiesDictionary.Add(recipe.EquipmentToCraft, recipe);   
        }

        public void Craft(EquipmentObject equipmentObject)
        {
            if(_recipiesDictionary.TryGetValue(equipmentObject, out var recipe))
            {
                if(HasRequiredMaterialsInBag(recipe))
                {
                    bool success = TryToCraftEquipmentFor(recipe); 
                    if(success)
                    {
                        GameObject craftedEquipment = CreateEquipment(recipe);
                        DestroyAndRemoveMaterialsFromCraftingBag(recipe);
                        AddCraftedEquipmentToEquipmentBag(craftedEquipment);
                        Debug.Log($"Crafted Succesfully item: {equipmentObject.Name}", craftedEquipment);
                        OnCraftingSuccess.Invoke(equipmentObject);
                        return;
                    }
                }

                OnCraftingFailure.Invoke(equipmentObject);
                Debug.Log($"Crafting failed for {equipmentObject.Name}");
            }

            OnCraftingFailure.Invoke(equipmentObject);
            Debug.LogError($"Equipment object {equipmentObject.Name} is not inside equipment dictionary, check recipes inside inspector!", gameObject);
        }

        private void DestroyAndRemoveMaterialsFromCraftingBag(CraftingRecipe recipe)
        {
            foreach(var material in recipe.Materials)
            {
                _equipmentBag.DestroyAndRemoveFromBag(material.Material, material.AmountNeeded);
            }
        }

        private void AddCraftedEquipmentToEquipmentBag(GameObject craftedEquipment)
        {
            _equipmentBag.AddToBag(craftedEquipment);
        }

        private GameObject CreateEquipment(CraftingRecipe recipe)
        {
            var equipment = Instantiate(recipe.Prefab,transform.position, Quaternion.identity);
            equipment.SetActive(false);
            return equipment;
        }

        private bool TryToCraftEquipmentFor(CraftingRecipe recipe)
        {
            if(recipe.ChanceOfSuccess == 0)
            {
                return false;
            }

            if(recipe.ChanceOfSuccess >= 1)
            {
                return true;
            } 

            var random = UnityEngine.Random.Range(0f, 1f);
            if(recipe.ChanceOfSuccess <=  random)
            {
                return true;
            }

            return false;
        }

        private bool HasRequiredMaterialsInBag(CraftingRecipe recipe)
        {
            foreach(var materials in recipe.Materials)
            {
                if(!_equipmentBag.ContainsEquipment(materials.Material,materials.AmountNeeded)) return false;
            }

            return true;
        }

    }
}
