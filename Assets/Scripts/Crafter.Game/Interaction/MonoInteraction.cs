using UnityEngine;

namespace Crafter.Game.Interaction
{
    public abstract class MonoInteraction : MonoBehaviour, Interactable
    {
        protected string _interactionName;
        public string InteractionName => _interactionName;

        public virtual void Ignore()
        {
            Debug.Log($"Lost for interaction: {InteractionName}", gameObject);
        }

        public virtual void Interact(InteractionArgs args)
        {
            Debug.Log($"Interacting with: {name}, via interaction: {InteractionName}",gameObject);
        }

        public virtual void Notice()
        {
            Debug.Log($"Noticed for interacion: {InteractionName}", gameObject);
        }
      
    }

}