using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Crafter.Game.Equipment
{
    public abstract class MonoEquipmentBag : MonoBehaviour, IEquipmentBag
    {
        [SerializeField] protected GameObject _equipmentPanel;

        protected Dictionary<EquipmentObject, IEquipmentBagSlot> _equipmentDictionary = new Dictionary<EquipmentObject, IEquipmentBagSlot> ();
        protected IEquipmentBagSlot[] _slots;

        [field: SerializeField] public UnityEvent<bool> OnPanelToggled { get; private set; }
        [field: SerializeField] public UnityEvent OnEquipmentPanelDisabled {  get; private set; }

        public void ToggleEquipmentPanel()
        {
            _equipmentPanel.SetActive(!_equipmentPanel.activeSelf);
            OnPanelToggled.Invoke(_equipmentPanel.gameObject.activeSelf);
        }

        public void DisableEequipmentPanel()
        {
            _equipmentPanel.SetActive(false);
            OnEquipmentPanelDisabled.Invoke();
        }

        public virtual void AddToBag(GameObject equipment)
        {
            var behaviour = equipment.GetComponent<EquipmentBehaviour>();
            if (behaviour == null)
            {
                Debug.LogError("Can't add to bag. No equipment behaviour provided for equipment gameObject", equipment);
                return;
            }

            var objectFromEquipment = behaviour.EquipmentObject;
            if (objectFromEquipment == null)
            {
                Debug.LogError("Can't add to bag. No equipment object provided for equipment behaviour in equipment gameObject", equipment);
                return;
            }

            if (_equipmentDictionary.TryGetValue(objectFromEquipment, out var bagSlot))
            {
                bagSlot.AddOne(equipment);
                Debug.Log($"Added equipment object to bag {objectFromEquipment.Name}", equipment);
                return;
            }

            bagSlot = _slots.FirstOrDefault(_item => _item.IsEmpty);
            if (bagSlot == null)
            {
                Debug.Log($"Can't add {objectFromEquipment.Name} to bag. No empty slots inside bag!");
                return;
            }

            bagSlot.AddOne(objectFromEquipment, equipment);
            _equipmentDictionary.Add(objectFromEquipment,bagSlot);
            Debug.Log($"Added equipment {objectFromEquipment.Name} to bag", equipment);
        }

        public virtual bool RemoveFromBag(GameObject equipment) 
        {
            var equipmentBehaviour = equipment.GetComponent<EquipmentBehaviour>();

            if (equipmentBehaviour == null)
            {
                Debug.LogError("No equipment behaviour provided can't remove from bag", equipment);
                return false;
            }

            if (equipmentBehaviour.EquipmentObject == null)
            {
                Debug.LogError("Can't remove. No equipment provided in equipment object", equipment);
                return false;
            }

            Debug.Log($"Removed equipment {equipmentBehaviour.EquipmentObject.Name} from bag", equipment);
            return true;
        }

        private void Start() { Init(); }

        protected virtual void Init()
        {
            foreach (var slot in _slots)
            {
                slot.OnSlotClicked.AddListener(OnBagSlotCliked);
            }
        }

        protected virtual void FindSlots()
        {
            _slots = _equipmentPanel.GetComponentsInChildren<IEquipmentBagSlot>(true);
        }

        public virtual void OnBagSlotCliked(IEquipmentBagSlot slot)
        {
            if (!slot.IsEmpty)
            {
                GameObject equipment = slot.RemoveOne();
                RemoveFromBag(equipment);
                if(slot.IsEmpty)
                {
                    var equipmentObject = equipment.GetComponent<EquipmentBehaviour>().EquipmentObject;
                    _equipmentDictionary.Remove(equipmentObject);
                }
            }
        }

        public bool ContainsEquipment(EquipmentObject equipment, int amount)
        {
            if(_equipmentDictionary.TryGetValue(equipment,out var slot))
            {
                if (slot.SlotCount < amount) return false;

                return true;
            }

            return false;
        }

        public void DestroyAndRemoveFromBag(EquipmentObject equipment, int amount)
        {
            if(_equipmentDictionary.TryGetValue(equipment,out var slot))
            {
                if(slot.SlotCount < amount)
                {
                    Debug.LogError($"Not enoguht items in equipment bag for {equipment.Name}. Required: {amount}, has: {slot.SlotCount}", gameObject);
                    return;
                }

                var gameObjects = slot.RemoveMany(equipment, amount);
                foreach(var gameObject in gameObjects)
                {
                    Destroy(gameObject);
                }

                if(slot.IsEmpty)
                {
                    _equipmentDictionary.Remove(equipment);
                }
            }
        }
    }
}
