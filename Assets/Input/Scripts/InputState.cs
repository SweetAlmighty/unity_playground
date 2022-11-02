using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
	/// <summary>
	///
	/// </summary>
	public enum InputType
	{
		State,
		Asset,
		Actions,
	}

	/// <summary>
	///
	/// </summary>
	public class InputState : MonoBehaviour
	{
		private Mouse mouse;
		private Vector2 rotation;
		private Vector3 moveDelta;
		private Keyboard keyboard;

		public void Awake()
		{
			this.mouse = Mouse.current;
			this.keyboard = Keyboard.current;
		}

		public void Update()
		{
			this.Look();
			this.Move();
		}

		private void Move()
		{
			this.moveDelta.z += Mathf.Max(this.keyboard.wKey.ReadValue(), this.keyboard.upArrowKey.ReadValue());
			this.moveDelta.x -= Mathf.Max(this.keyboard.aKey.ReadValue(), this.keyboard.leftArrowKey.ReadValue());
			this.moveDelta.z -= Mathf.Max(this.keyboard.sKey.ReadValue(), this.keyboard.downArrowKey.ReadValue());
			this.moveDelta.x += Mathf.Max(this.keyboard.dKey.ReadValue(), this.keyboard.rightArrowKey.ReadValue());

			if (this.moveDelta == Vector3.zero)
				return;

			Vector3 move = Quaternion.Euler(0, this.transform.eulerAngles.y, 0) * this.moveDelta;

			this.gameObject.transform.position += move * (Time.deltaTime * 10);
			this.moveDelta = Vector3.zero;
		}

		private void Look()
		{
			Vector2 lookDelta = this.mouse.delta.ReadValue();

			if (lookDelta.sqrMagnitude < 0.01)
				return;

			float rotateSpeed = 60 * Time.deltaTime;
			this.rotation.y += lookDelta.x * rotateSpeed;
			this.rotation.x = Mathf.Clamp(this.rotation.x - lookDelta.y * rotateSpeed, -89, 89);

			this.transform.localEulerAngles = this.rotation;
		}
	}
}
