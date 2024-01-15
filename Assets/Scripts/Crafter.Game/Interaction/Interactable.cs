namespace Crafter.Game.Interaction
{
    public interface Interactable
    {
        public void Interact(InteractionArgs args);
        public void Notice();
        public void Ignore();
    }

}