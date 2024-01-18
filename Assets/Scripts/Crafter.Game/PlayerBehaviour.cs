using Crafter.Game.Equipment;
using Crafter.Game.Interaction;
using System.Collections.Generic;
using UnityEngine;

namespace Crafter.Game
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera _gameCamera;

        [Header("Movement")]
        [SerializeField] private float _playerSpeed = 5.0f;
        [SerializeField] private float _jumpForce = 1.0f;
        [SerializeField] private bool _canMove = true;

        [Header("Input")]
        [SerializeField] private KeyCode _backToMenuKey = KeyCode.Escape;
        [SerializeField] private KeyCode _interactKey = KeyCode.E;
        [SerializeField] private KeyCode _openBagKey = KeyCode.B;
        [SerializeField] private KeyCode _jumpKeyCode = KeyCode.Space;

        private EquipmentBag _bag;
        private CharacterController _controller;
        private Animator _animator;
        private InteractionZone _interactionZone;

        private Vector3 _playerVelocity;
        private bool _groundedPlayer;
        private float _gravityValue = -9.81f;
        private LinkedList<Interactable> _interactables;

        public IEquipmentBag EquipmentBag => _bag;


        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _interactionZone = GetComponentInChildren<InteractionZone>();
            _bag = gameObject.GetComponent<EquipmentBag>();
            _interactables = new LinkedList<Interactable>();
            _interactionZone.OnInteractionNotice.AddListener(OnInteractionNotice);
            _interactionZone.OnInteractionIgnore.AddListener(OnInteractionIgnore);

            _bag.OnPanelToggled.AddListener(OnEquipmentPanelToggled);
        }

        private void OnEquipmentPanelToggled(bool active)
        {
            _canMove = !active;
        }

        private void OnInteractionIgnore(Interactable interaction)
        {
            _interactables.Remove(interaction);
        }

        private void OnInteractionNotice(Interactable interaction)
        {
            _interactables.AddLast(interaction);
        }

        void Update()
        {
            HandleMovement();
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(_backToMenuKey))
            {
                GameManager.Instance.BackToMenu();
            }

            if (Input.GetKeyDown(_interactKey))
            {
                Interact();
            }

            if (Input.GetKeyDown(_openBagKey))
            {
                _bag.ToggleEquipmentPanel();
            }
        }

        private void HandleMovement()
        {
            _groundedPlayer = _controller.isGrounded;

            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -0.5f;
            }

            Vector3 input = Vector3.zero;
            if (_canMove) { input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); }

            Vector3 move;
            var forward = _gameCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            var right = Vector3.Cross(Vector3.up, forward);
            move = forward * input.z + right * input.x;

            move.y = 0;

            _controller.Move(move * Time.deltaTime * _playerSpeed);

            _animator.SetFloat("MovementX", input.x);
            _animator.SetFloat("MovementZ", input.z);

            if (input != Vector3.zero)
            {
                gameObject.transform.forward = forward;
            }

            if (Input.GetKeyDown(_jumpKeyCode) && _groundedPlayer && _canMove)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpForce * -3.0f * _gravityValue);
                _animator.SetTrigger("Jump");
            }

            _playerVelocity.y += _gravityValue * Time.deltaTime;

            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void Interact()
        {
            if (_interactables.Count > 0)
            {
                InteractionArgs args = new InteractionArgs() { Subject = gameObject };
                _interactables.First.Value.Interact(args);
                _interactables.RemoveFirst();
            }
        }


    }

}