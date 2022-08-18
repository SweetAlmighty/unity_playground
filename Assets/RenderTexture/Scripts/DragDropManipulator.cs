using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.RenderTexture
{
	/// <summary>
	/// Helper class that handles logic for dragging a <see cref="VisualElement"/> within the scene.
	/// <remarks>
	/// Reference: https://docs.unity3d.com/2021.2/Documentation/Manual/UIE-create-drag-and-drop-ui.html
	/// </remarks>
	/// </summary>
	public class DragDropManipulator : PointerManipulator
	{
		/// <summary>
		/// Whether the move logic should be run.
		/// </summary>
		private bool Enabled { get; set; }

		/// <summary>
		/// The <see cref="VisualElement"/> that will be moved with the user's input.
		/// </summary>
		private VisualElement Root { get; }

		/// <summary>
		/// The position of <see cref="Root"/> when the drag was started.
		/// </summary>
		private Vector2 TargetStartPosition { get; set; }

		/// <summary>
		/// The user's pointer position when the drag was started.
		/// </summary>
		private Vector3 PointerStartPosition { get; set; }

		/// <summary>
		/// Constructors a new instance of <see cref="DragDropManipulator"/>.
		/// </summary>
		/// <param name="target">The <see cref="VisualElement"/> that will trigger the drag logic.</param>
		public DragDropManipulator(VisualElement target)
		{
			this.target = target;
			this.Root = target.parent;
		}

		/// <summary>
		/// Registers callbacks to the <see cref="PointerUpEvent"/>, <see cref="PointerDownEvent"/>, and
		/// <see cref="PointerMoveEvent"/> events of <see cref="Target"/>.
		/// </summary>
		protected override void RegisterCallbacksOnTarget()
		{
			this.target.RegisterCallback<PointerUpEvent>(this.PointerUpHandler);
			this.target.RegisterCallback<PointerDownEvent>(this.PointerDownHandler);
			this.target.RegisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
		}

		/// <summary>
		/// Unregisters callbacks to the <see cref="PointerUpEvent"/>, <see cref="PointerDownEvent"/>, and
		/// <see cref="PointerMoveEvent"/> events of <see cref="Target"/>.
		/// </summary>
		protected override void UnregisterCallbacksFromTarget()
		{
			this.target.UnregisterCallback<PointerUpEvent>(this.PointerUpHandler);
			this.target.UnregisterCallback<PointerDownEvent>(this.PointerDownHandler);
			this.target.UnregisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
		}

		/// <summary>
		/// Callback for <see cref="PointerDownEvent"/> that stores the start positions of the mouse cursor and <see cref="Target"/>.
		/// </summary>
		/// <param name="pointerDownEvent">Container for data related to the event.</param>
		private void PointerDownHandler(PointerDownEvent pointerDownEvent)
		{
			this.Enabled = true;
			this.PointerStartPosition = pointerDownEvent.position;
			this.target.CapturePointer(pointerDownEvent.pointerId);
			this.TargetStartPosition = this.Root.transform.position;
		}

		/// <summary>
		/// Callback for <see cref="PointerMoveEvent"/> that calculates the delta between the positions captured in
		/// <see cref="PointerDownHandler"/> and the pointer's current position.
		/// </summary>
		/// <param name="pointerMoveEvent">Container for data related to the event.</param>
		private void PointerMoveHandler(PointerMoveEvent pointerMoveEvent)
		{
			if (!this.Enabled || !this.target.HasPointerCapture(pointerMoveEvent.pointerId))
				return;

			Vector3 pointerDelta = pointerMoveEvent.position - this.PointerStartPosition +
			                       new Vector3(this.TargetStartPosition.x, this.TargetStartPosition.y);
			this.Root.transform.position = new Vector2(Mathf.Clamp(pointerDelta.x, 0, this.Root.panel.visualTree.worldBound.width),
			                                           Mathf.Clamp(pointerDelta.y, 0, this.target.panel.visualTree.worldBound.height));
		}

		/// <summary>
		/// Callback for <see cref="PointerUpEvent"/> that releases the pointer from <see cref="Target"/>.
		/// </summary>
		/// <param name="pointerUpEvent">Container for data related to the event.</param>
		private void PointerUpHandler(PointerUpEvent pointerUpEvent)
		{
			if (!this.Enabled || !this.target.HasPointerCapture(pointerUpEvent.pointerId))
				return;

			this.Enabled = false;
			this.target.ReleasePointer(pointerUpEvent.pointerId);
		}
	}
}
