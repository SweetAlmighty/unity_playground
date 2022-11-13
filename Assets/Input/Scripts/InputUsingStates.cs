using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
	/// <summary>
	/// Wrapper component for manipulating the scene using states held by Unity's references to input devices.
	/// </summary>
	public class InputUsingStates : BaseInput
	{
        /// <summary>
		/// The user's current Mouse input device.
        /// </summary>
        private Mouse currentMouse;

		/// <summary>
		/// The user's current Keyboard input device.
		/// </summary>
		private Keyboard currentKeyboard;

        /// <summary>
        /// Captures the user's current Keyboard and Mouse devices.
        /// </summary>
        private void Awake()
		{
			this.currentMouse = Mouse.current;
			this.currentKeyboard = Keyboard.current;
        }

        /// <summary>
        /// Runs the player's looking anhd movement logic.
        /// </summary>
        private void Update()
        {
            this.DetermineLookDelta();
            this.DetermineMoveDelta();
        }

        /// <summary>
        /// Calculates the player's look delta vector, if any look input has been received.
        /// </summary>
        private void DetermineLookDelta()
        {
            Vector2 lookDelta = Vector2.zero;

			if (this.currentMouse.wasUpdatedThisFrame)
				lookDelta = this.currentMouse.delta.ReadValue();

			if (lookDelta.sqrMagnitude >= 0.01f)
				this.player.Look(lookDelta);
		}

        /// <summary>
        /// Calculates the player's move delta vector, if any movement input has been received.
        /// </summary>
        private void DetermineMoveDelta()
		{
            Vector3 moveDelta = Vector3.zero;

            if (this.currentKeyboard.wKey.isPressed)
                moveDelta.z += this.currentKeyboard.wKey.ReadValue();

            if (this.currentKeyboard.aKey.isPressed)
                moveDelta.x -= this.currentKeyboard.aKey.ReadValue();

            if (this.currentKeyboard.sKey.isPressed)
                moveDelta.z -= this.currentKeyboard.sKey.ReadValue();

            if (this.currentKeyboard.dKey.isPressed)
                moveDelta.x += this.currentKeyboard.dKey.ReadValue();

            if (moveDelta != Vector3.zero)
                this.player.Move(moveDelta);
        }
	}
}
