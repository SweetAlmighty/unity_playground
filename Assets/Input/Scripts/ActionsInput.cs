using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
    /// <summary>
    /// A derived input component that handles triggering movement logic for the player.
    /// </summary>
    public class ActionsInput : BaseInput
    {
        /// <summary>
        /// The flag that is checked within <see cref="Update"/> to determine if there is a
        /// value within <see cref="movementDelta"/> that can be applied to the player.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// A delta captured the player's input to apply to the player character.
        /// </summary>
        private Vector3 movementDelta = Vector3.zero;

        /// <summary>
        /// Checks <see cref="isMoving"/> to determine if <see cref="movementDelta"/> should be applied to the player.
        /// </summary>
        private void Update()
        {
            if (this.isMoving)
                this.player.Move(this.movementDelta);
        }

        /// <summary>
        /// Callback that captures values used to move the player within the scene.
        /// </summary>
        /// <param name="context">Data object that contains values we will use to move the player character in the scene.</param>
        protected void OnMove(InputAction.CallbackContext context)
        {
            this.isMoving = context.performed;
            this.movementDelta = context.ReadValue<Vector3>();
        }
    }
}
