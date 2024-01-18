using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafter.Game.Equipment
{
    public class EquipmentBagSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private EquipmentObject _equipment;
        [SerializeField] private List<GameObject> _equipmentGameObjects = new List<GameObject>();

        public bool IsEmpty => _equipmentGameObjects.Count == 0 && _equipment == null;
        public EquipmentObject Equipment => _equipment;

        public UnityEvent<EquipmentBagSlot> OnSlotClicked;

        public void AddToSlot(GameObject gameObject)
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
                _equipment = null;
                _image.sprite = null;
                _image.raycastTarget = false;
            }

            return gameObject;
        }

        public void AddToSlot(EquipmentObject equipmentObject, GameObject gameObject)
        {
            AddToSlot(gameObject);
            _equipment = equipmentObject;
            _image.sprite = equipmentObject.Icon;
            _image.raycastTarget = true;
        }

        public bool ContainsEquipment(EquipmentObject equipmentObject)
        {
            if (equipmentObject == null || _equipment == null) return false;

            return _equipment.Equals(equipmentObject);
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

    }
}
