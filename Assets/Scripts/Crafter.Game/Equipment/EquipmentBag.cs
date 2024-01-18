using UnityEngine;

namespace Crafter.Game.Equipment
{
    public class EquipmentBag : MonoEquipmentBag
    {
        [Tooltip("How far removed object should, be next to player"), SerializeField] private float _throwRadius = 2f;

        public override void RemoveFromBag(GameObject gameObject)
        {
            var equipmentBehaviour = gameObject.GetComponent<EquipmentBehaviour>();

            if (equipmentBehaviour == null)
            {
                Debug.LogError("No equipment behaviour provided can't remove from bag", gameObject);
                return;
            }

            if (equipmentBehaviour.EquipmentObject == null)
            {
                Debug.LogError("Can't remove. No equipment provided in equipment object", gameObject);
                return;
            }

            gameObject.RotateRandomAround(this.gameObject, _throwRadius);
            gameObject.SetActive(true);

            Debug.Log($"Removed equipment {equipmentBehaviour.EquipmentObject.Name} from bag", gameObject);
        }
    }
}
