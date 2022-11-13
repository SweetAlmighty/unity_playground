using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
    /// <summary>
	/// Wrapper component for manipulating the scene using <see cref="InputAction"/> references configured wihtin the inspector.
    /// </summary>
    public class InputUsingActions : ActionsInput
    {
        [SerializeField]
        [Tooltip("The action used to capture input used to move the player in the scene.")]
        private InputAction moveAction;

        [SerializeField]
        [Tooltip("The action used to capture input used to manipulate the player's camera in the scene.")]
        private InputAction lookAction;

        [SerializeField]
        [Tooltip("The action used to capture input used to make the player perform a jump action in the scene.")]
        private InputAction jumpAction;

        /// <summary>
        /// Enables the <see cref="InputAction"/> references on this component.
        /// </summary>
        private void OnEnable()
        {
            this.moveAction.Enable();
            this.lookAction.Enable();
            this.jumpAction.Enable();
        }

        /// <summary>
        /// Disables the <see cref="InputAction"/> references on this component.
        /// </summary>
        private void OnDisable()
        {
            this.moveAction.Disable();
            this.lookAction.Disable();
            this.jumpAction.Disable();
        }

        /// <summary>
        /// Subscribes our player's manipulation methods to the appropriate <see cref="InputAction"/> references.
        /// </summary>
        private void Awake()
        {
            this.moveAction.performed += OnMove;
            this.lookAction.performed += player.Look;
            this.jumpAction.performed += player.Jump;
        }
    }
}
