using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
	/// <summary>
	///
	/// </summary>
	public class InputUsingStates : BaseInput
	{
        /// <summary>
        /// 
        /// </summary>
        private Mouse currentMouse;

		/// <summary>
		/// 
		/// </summary>
		private Keyboard currentKeyboard;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
		{
			this.currentMouse = Mouse.current;
			this.currentKeyboard = Keyboard.current;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            this.DetermineLookDelta();
            this.DetermineMoveDelta();
        }

        /// <summary>
        /// 
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
        /// 
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
