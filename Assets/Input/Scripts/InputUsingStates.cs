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
		public override Vector2 DetermineLookDelta() => this.Look(this.currentMouse.delta.ReadValue());

        /// <summary>
        /// 
        /// </summary>
        public override Vector3 DetermineMoveDelta()
		{
			Vector3 moveDelta = Vector3.zero;
			moveDelta.z += Mathf.Max(this.currentKeyboard.wKey.ReadValue(), this.currentKeyboard.upArrowKey.ReadValue());
			moveDelta.x -= Mathf.Max(this.currentKeyboard.aKey.ReadValue(), this.currentKeyboard.leftArrowKey.ReadValue());
			moveDelta.z -= Mathf.Max(this.currentKeyboard.sKey.ReadValue(), this.currentKeyboard.downArrowKey.ReadValue());
			moveDelta.x += Mathf.Max(this.currentKeyboard.dKey.ReadValue(), this.currentKeyboard.rightArrowKey.ReadValue());
			return moveDelta;
		}
	}
}
