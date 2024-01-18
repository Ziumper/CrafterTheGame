using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafter.Game.Equipment
{
    public class EquipmentBagSlot : MonoBehaviour, IPointerClickHandler, IEquipmentBagSlot
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private EquipmentObject _equipment;
        [SerializeField] private List<GameObject> _equipmentGameObjects = new List<GameObject>();
        
        [field: SerializeField] public UnityEvent<IEquipmentBagSlot> OnSlotClicked { get; private set; }

        public int SlotCount => _equipmentGameObjects.Count;
        public bool IsEmpty => _equipmentGameObjects.Count == 0 && _equipment == null;
        public EquipmentObject Equipment => _equipment;

        public void AddOne(GameObject gameObject)
        {
            _equipmentGameObjects.Add(gameObject);
            UpdateCountText();
        }

        public GameObject RemoveOne()
        {
            var gameObject = _equipmentGameObjects.FirstOrDefault();
            
            if(_equipmentGameObjects.Count > 0)
            {
                if(gameObject != null) { _equipmentGameObjects.Remove(gameObject); }
            }
            
            UpdateCountText();

            if(_equipmentGameObjects.Count <= 0)
            {
                ResetSlot();
            }

            return gameObject;
        }

        public void AddOne(EquipmentObject equipmentObject, GameObject gameObject)
        {
            AddOne(gameObject);
            _equipment = equipmentObject;
            _image.sprite = equipmentObject.Icon;
            _image.raycastTarget = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_equipment == null)
            {
                Debug.Log("Clicked on empty equipment slot", gameObject);
                return;
            }

            OnSlotClicked.Invoke(this);
        }

        private void UpdateCountText()
        {
            if (_countText == null)
            {
                Debug.LogError("No count text assigned to bag slot", gameObject);
                return;
            }

            if (_equipmentGameObjects.Count <= 0)
            {
                _countText.text = string.Format("{0}", 0);
                _countText.gameObject.SetActive(false);
                return;
            }

            if (!_countText.gameObject.activeSelf)
            {
                _countText.gameObject.SetActive(true);
            }

            _countText.text = _equipmentGameObjects.Count.ToString();
        }

        public List<GameObject> RemoveMany(EquipmentObject equipmentObject, int amount)
        {
            var start = 0;
            var gameObjects = _equipmentGameObjects.GetRange(start, amount);
            _equipmentGameObjects.RemoveRange(start,amount);

            UpdateCountText();

            if (_equipmentGameObjects.Count <= 0) 
            {
                 ResetSlot();   
            }

            return gameObjects;
        }

        private void ResetSlot()
        {
            _equipment = null;
            _image.sprite = null;
            _image.raycastTarget = false;
        }
    }
}
