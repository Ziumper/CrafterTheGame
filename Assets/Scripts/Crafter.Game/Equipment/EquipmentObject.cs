using UnityEngine;

namespace Crafter.Game.Equipment
{
    [CreateAssetMenu(fileName = "Equipment", menuName ="Crafter/Equipment")]
    public class EquipmentObject : ScriptableObject
    {
        [field: SerializeField]
        public string Name {  get; set; }
        [field: SerializeField]
        public Sprite Icon { get; set; }
    }
}
