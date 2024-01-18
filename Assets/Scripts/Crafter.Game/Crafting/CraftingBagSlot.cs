using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafter.Game.Crafting
{
    public class CraftingBagSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _chanceOfSuccess;
        [field: SerializeField] public CraftingRecipe Recipe { get; private set; }
        [field: SerializeField] public UnityEvent<CraftingBagSlot> OnSlotClicked { get; private set; }

        void Start()
        {
            _image = GetComponent<Image>();
            _image.sprite = Recipe.EquipmentToCraft.Icon;
            _chanceOfSuccess.text = string.Format("{0}%", Recipe.ChanceOfSuccess*100);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotClicked.Invoke(this);
        }
    }
}
