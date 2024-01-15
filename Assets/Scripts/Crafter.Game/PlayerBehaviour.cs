using Crafter.Game.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Crafter.Game
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _playerSpeed = 5.0f;
        [SerializeField] private float _jumpForce = 1.0f;

        [Header("Equipment Bag")]
        [SerializeField] private List<GameObject> _equipment = new List<GameObject>();

        private Camera _gameCamera;
        private CharacterController _controller;
        private Animator _animator;
        private InteractionZone _interactionZone;
        
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;
        private float _gravityValue = -9.81f;
        private LinkedList<Interactable> _interactables;
        

        private void Start()
        {
            _gameCamera = Camera.main;
            _controller = gameObject.GetComponent<CharacterController>();
            _animator = gameObject.GetComponentInChildren<Animator>();
            _interactionZone = gameObject.GetComponentInChildren<InteractionZone>();
            
            _interactables = new LinkedList<Interactable>();
            _interactionZone.OnInteractionNotice.AddListener(OnInteractionNotice);
            _interactionZone.OnInteractionIgnore.AddListener(OnInteractionIgnore);
        }

        private void OnDestroy()
        {
            _equipment.Clear();
        }

        private void OnInteractionIgnore(Interactable interaction)
        {
            _interactables.Remove(interaction);
        }

        private void OnInteractionNotice(Interactable interaction)
        {
            _interactables.AddFirst(interaction);
        }

        void Update()
        {
            HandleMovement();
            HandleInput();
        }


        private void HandleInput()
        {
            if(Input.GetButtonDown("Cancel")) {
                GameManager.Instance.BackToMenu();
            }

            if(Input.GetButtonDown("Submit"))
            {
                Interact();
            }
        }

        private void HandleMovement()
        {
            _groundedPlayer = _controller.isGrounded;

            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -0.5f;
            }

            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //trasnform input into camera space
            var forward = _gameCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();
            var right = Vector3.Cross(Vector3.up, forward);

            Vector3 move = forward * input.z + right * input.x;
            move.y = 0;

            _controller.Move(move * Time.deltaTime * _playerSpeed);

            _animator.SetFloat("MovementX", input.x);
            _animator.SetFloat("MovementZ", input.z);

            if (input != Vector3.zero)
            {
                gameObject.transform.forward = forward;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpForce * -3.0f * _gravityValue);
                _animator.SetTrigger("Jump");
            }

            _playerVelocity.y += _gravityValue * Time.deltaTime;

            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        public void AddToBag(GameObject gameObject)
        {
            _equipment.Add(gameObject);
        }
        
        private void Interact()
        {
            if (_interactables.Count > 0)
            {
                Debug.Log("Interacting");
                InteractionArgs args = new InteractionArgs() { Subject = gameObject };
                _interactables.First.Value.Interact(args);
                _interactables.RemoveFirst();
            }
        }

    }

}