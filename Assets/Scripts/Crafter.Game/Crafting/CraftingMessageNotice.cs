using Crafter.Game.Equipment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    public class CraftingMessageNotice : MonoBehaviour
    { 
        [SerializeField] private float _waitTime;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField, TextArea] private string _successText;
        [SerializeField, TextArea] private string _failedText;
        [SerializeField, TextArea] private string _noMaterialsText;

        private readonly HashSet<string> _objectsSet = new ();
        private readonly Queue<string> _objectsQueue = new ();
        private bool _isPlaying = false;


        public void OnSuccess(EquipmentObject equipmentObject)
        {
            AppendMessageAndShowNotice(_successText, equipmentObject);
        }

        public void OnFailure(EquipmentObject equipmentObject)
        {
            AppendMessageAndShowNotice(_failedText, equipmentObject);
        }

        public void OnNNoMaterialsText(EquipmentObject equipmentObject)
        {
            AppendMessageAndShowNotice(_noMaterialsText, equipmentObject);
        }

        private void AppendMessageAndShowNotice(string message, EquipmentObject equipment)
        {
            var messageToShow = message + equipment.Name;
            ShowNoticeMessage(messageToShow);
        }

        private void ShowNoticeMessage(string message)
        {
            if(_objectsSet.Contains(message))
            {
                return;
            } else
            {
                _objectsSet.Add(message);
                _objectsQueue.Enqueue(message);
            }
            
            if (_isPlaying) { return; }
            gameObject.SetActive(true);
            StartCoroutine(WaitForEnd());
        }

        private IEnumerator WaitForEnd()
        {
            _isPlaying = true;
            while(_objectsQueue.TryDequeue(out var message)) 
            {
                _message.text = message;
                yield return new WaitForSeconds(_waitTime);
                _objectsSet.Remove(message);
            }

            _isPlaying = false;
            gameObject.SetActive(false);
        }

    }
}
