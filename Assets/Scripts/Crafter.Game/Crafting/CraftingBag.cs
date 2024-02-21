using Crafter.Game.Equipment;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Crafting
{
    public class CraftingBag : MonoBehaviour, ICraftingBag
    {
        [Serializable]
        public class CraftingEvents
        {
            public UnityEvent<EquipmentObject> OnSuccess;
            public UnityEvent<EquipmentObject> OnFailure;
            public UnityEvent<EquipmentObject> OnNotEnoughMaterials;
        }

        [SerializeField] private IEquipmentBag _equipmentBag;
        [SerializeField] private CraftingBagSlot[] _craftingSlots;
        [SerializeField] private GameObject _creaftingPanel;

        [SerializeField] private CraftingEvents _craftingEvents;

        void Start()
        {
            foreach (var slot in _craftingSlots)
            {
                slot.OnSlotClicked.AddListener(OnCraftingSlotCliked);
            }

            _equipmentBag = gameObject.GetComponent<EquipmentBag>();
            _equipmentBag.OnPanelToggled.AddListener(OnEquipmentPanelToggled);
            _equipmentBag.OnEquipmentPanelDisabled.AddListener(OnEquipmentPanelDisabled);
        }

        private void OnEquipmentPanelDisabled()
        {
            _creaftingPanel.SetActive(false);
        }

        private void OnEquipmentPanelToggled(bool isActive)
        {
            if (!isActive) { _creaftingPanel.SetActive(false); }
        }

        [ContextMenu("Find crafting slots")]
        private void FindCraftingSlots()
        {
            _craftingSlots = _creaftingPanel.GetComponentsInChildren<CraftingBagSlot>();
        }

        private void OnCraftingSlotCliked(CraftingBagSlot slot)
        {
            CraftFrom(slot.Recipe);
        }

        public void CraftFrom(CraftingRecipe recipe)
        {
            if (HasRequiredMaterialsInBag(recipe))
            {
                bool success = TryToCraftEquipmentFor(recipe);
                if (success)
                {
                    GameObject craftedEquipment = CreateEquipment(recipe);
                    DestroyAndRemoveMaterialsFromCraftingBag(recipe);
                    AddCraftedEquipmentToEquipmentBag(craftedEquipment);
                    Debug.Log($"Crafted Succesfully item: {recipe.EquipmentToCraft.Name}", craftedEquipment);
                    _craftingEvents.OnSuccess.Invoke(recipe.EquipmentToCraft);
                    return;
                }

                _craftingEvents.OnFailure.Invoke(recipe.EquipmentToCraft);
                DestroyAndRemoveMaterialsFromCraftingBag(recipe);
                Debug.Log($"Crafting failed for {recipe.EquipmentToCraft.Name}");
                return;
            }

            _craftingEvents.OnNotEnoughMaterials.Invoke(recipe.EquipmentToCraft);
            Debug.Log($"Not enough resources for recipe: {recipe.EquipmentToCraft.Name}!", gameObject);
        }

        private void DestroyAndRemoveMaterialsFromCraftingBag(CraftingRecipe recipe)
        {
            foreach (var material in recipe.Materials)
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
            var equipment = Instantiate(recipe.Prefab, transform.position, Quaternion.identity);
            equipment.SetActive(false);
            return equipment;
        }

        private bool TryToCraftEquipmentFor(CraftingRecipe recipe)
        {
            if (recipe.ChanceOfSuccess == 0)
            {
                return false;
            }

            if (recipe.ChanceOfSuccess >= 1)
            {
                return true;
            }

            var random = UnityEngine.Random.Range(0f, 1f);
            if (random <= recipe.ChanceOfSuccess)
            {
                return true;
            }

            return false;
        }

        private bool HasRequiredMaterialsInBag(CraftingRecipe recipe)
        {
            foreach (var materials in recipe.Materials)
            {
                if (!_equipmentBag.ContainsEquipment(materials.Material, materials.AmountNeeded)) return false;
            }

            return true;
        }

    }
}
