using Crafter.Game.Interaction;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Crafter.Game.Interaction
{
    public class InteractionZone : MonoBehaviour
    {
        public UnityEvent<Interactable> OnInteractionNotice;
        public UnityEvent<Interactable> OnInteractionIgnore;

        private void OnTriggerEnter(Collider other)
        {
            var interaction = other.GetComponent<MonoInteraction>();
            if (interaction != null)
            {
                interaction.Notice();
                OnInteractionNotice.Invoke(interaction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interaction = other.GetComponent<MonoInteraction>();
            if (interaction != null)
            {
                interaction.Ignore();
                OnInteractionIgnore.Invoke(interaction);
            }
        }
    }

}