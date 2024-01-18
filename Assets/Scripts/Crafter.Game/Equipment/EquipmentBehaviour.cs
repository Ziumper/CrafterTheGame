using Crafter.Game.Interaction;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter.Game.Equipment
{
    public class EquipmentBehaviour : MonoInteraction
    {
        private Outline _outline;

        [field: SerializeField] public EquipmentObject EquipmentObject { get; private set; }
        
        void Start()
        {
            _interactionName = "Pickup";
            _outline = GetComponent<Outline>();
            _outline.enabled = false;

            var icons = GetComponentsInChildren<SpriteRenderer>();
            foreach ( var icon in icons )
            {
                icon.sprite = EquipmentObject.Icon;
            }
        }

        public override void Notice()
        {
            base.Notice();
            _outline.enabled = true;
        }

        public override void Ignore()
        {
            base.Ignore();
            _outline.enabled = false;
        }

        public override void Interact(InteractionArgs args)
        {
            base.Interact(args);

            var player = args.Subject.GetComponent<PlayerBehaviour>();

            gameObject.SetActive(false);
            player.EquipmentBag.AddToBag(gameObject);
            _outline.enabled = false;
        }
    }
}