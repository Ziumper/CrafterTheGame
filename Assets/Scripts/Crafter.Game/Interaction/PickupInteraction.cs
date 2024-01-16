namespace Crafter.Game.Interaction
{
    public class PickupInteraction : MonoInteraction
    {
        private Outline _outline;

        void Start()
        {
            _interactionName = "Pickup";
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
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

            //do something with player add object to bag
            var player = args.Subject.GetComponent<PlayerBehaviour>();
            
            gameObject.SetActive(false);
            player.AddToBag(gameObject);
        }
    }

}