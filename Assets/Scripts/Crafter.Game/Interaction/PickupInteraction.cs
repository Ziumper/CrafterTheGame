namespace Crafter.Game.Interaction
{
    public class PickupInteraction : MonoInteraction
    {
        void Start()
        {
            _interactionName = "Pickup";
        }

        public override void Interact(InteractionArgs args)
        {
            base.Interact(args);

            //do something with player add object to bag
            var player = args.Subject.GetComponent<PlayerBehaviour>();
            
            gameObject.SetActive(false);
            player.AddToBag(gameObject);
        }
    }

}