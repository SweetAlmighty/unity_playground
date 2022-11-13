using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
    /// <summary>
	/// Wrapper component for manipulating the scene using a custom <see cref="InputAction"/> asset (<see cref="ActionAsset"/>).
    /// </summary>
    public class InputUsingActionAsset : ActionsInput
    {
        /// <summary>
        /// Reference to the asset containing the <see cref="InputAction"/> configurations.
        /// </summary>
        private ActionAsset controls;

        /// <summary>
        /// Enables <see cref="controls"/> when the component is enabled.
        /// </summary>
        private void OnEnable() => this.controls.Enable();

        /// <summary>
        /// Disables <see cref="controls"/> when the component is Disables.
        /// </summary>
        private void OnDisable() => this.controls.Disable();

        /// <summary>
        /// Initialized the <see cref="ActionAsset"/> instance, and subscribes our player's manipulation methods to the appropriate <see cref="InputAction"/> references.
        /// </summary>
        private void Awake()
        {
            this.controls = new ActionAsset();
            this.controls.gameplay.Move.performed += OnMove;
            this.controls.gameplay.Jump.performed += player.Jump;
            this.controls.gameplay.Look.performed += player.Look;
        }
    }
}
