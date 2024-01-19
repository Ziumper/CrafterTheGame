using Cinemachine;
using Crafter.Game.Equipment;
using Crafter.Game.Interaction;
using System.Collections;
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
        private CinemachineFreeLook _freeLook;

        private Vector3 _playerVelocity;
        private bool _groundedPlayer;
        private float _gravityValue = -9.81f;
        private LinkedList<Interactable> _interactables;
        private Vector3 _inputVector;
        private Vector3 _moveVector;
        private Vector3 _forwardVector;

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
            _freeLook = GetComponentInChildren<CinemachineFreeLook>();

            _bag.OnPanelToggled.AddListener(OnEquipmentPanelToggled);
        }

        private void OnEquipmentPanelToggled(bool active)
        {
            StartCoroutine(ToggleCursor(active));
        }

        private IEnumerator ToggleCursor(bool active)
        {
            _canMove = !active;

            if (active) { GameManager.Instance.ShowCursor(); }
            else { GameManager.Instance.HideCursor(); }

            yield return new WaitForEndOfFrame();

            _freeLook.enabled = !active;
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
            UpdateMovement();
            UpdateInput();
        }

        private void UpdateInput()
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

        private void UpdateMovement()
        {
            GuardPlayerGround();
            UpdateMoveVectors();
            HandleJumping();
        }

        private void UpdateMoveVectors()
        {
            UpdateInputVector();
           
            var _forwardVector = _gameCamera.transform.forward;
            _forwardVector.y = 0;
            _forwardVector.Normalize();

            var right = Vector3.Cross(Vector3.up, _forwardVector);
            _moveVector = _forwardVector * _inputVector.z + right * _inputVector.x;
            _moveVector.y = 0;

            _controller.Move(_moveVector * Time.deltaTime * _playerSpeed);

            if (_inputVector != Vector3.zero)
            {
                gameObject.transform.forward = _forwardVector;
            }

            AnimateMovement();
        }

        private void AnimateMovement()
        {
            _animator.SetFloat("MovementX", _inputVector.x);
            _animator.SetFloat("MovementZ", _inputVector.z);
        }

        private void UpdateInputVector()
        {
            _inputVector = Vector3.zero;
            if (_canMove) { _inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); }
        }

        private void GuardPlayerGround()
        {
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -0.5f;
            }
        }

        private void HandleJumping()
        {
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