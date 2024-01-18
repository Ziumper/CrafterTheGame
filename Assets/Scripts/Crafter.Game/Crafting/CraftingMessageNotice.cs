using Crafter.Game.Equipment;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Crafter.Game.Crafting
{
    public class CraftingMessageNotice : MonoBehaviour
    { 
        [SerializeField] private float _waitTime;
        [SerializeField] private TextMeshProUGUI _item;

        public void ShowNoticeMessage(EquipmentObject equipment)
        {
            _item.text += equipment.Name;
            gameObject.SetActive(true);
            StartCoroutine(WaitForEnd());
        }

        private IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(_waitTime);
           gameObject.SetActive(false);
        }

    }
}
