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
		///
		/// </summary>
		private bool Enabled { get; set; }

		/// <summary>
		///
		/// </summary>
		private VisualElement Root { get; }

		/// <summary>
		///
		/// </summary>
		private Vector2 TargetStartPosition { get; set; }

		/// <summary>
		///
		/// </summary>
		private Vector3 PointerStartPosition { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="target"></param>
		public DragDropManipulator(VisualElement target)
		{
			this.target = target;
			this.Root = target.parent;
		}

		/// <summary>
		///
		/// </summary>
		protected override void RegisterCallbacksOnTarget()
		{
			this.target.RegisterCallback<PointerUpEvent>(this.PointerUpHandler);
			this.target.RegisterCallback<PointerDownEvent>(this.PointerDownHandler);
			this.target.RegisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
		}

		/// <summary>
		///
		/// </summary>
		protected override void UnregisterCallbacksFromTarget()
		{
			this.target.UnregisterCallback<PointerUpEvent>(this.PointerUpHandler);
			this.target.UnregisterCallback<PointerDownEvent>(this.PointerDownHandler);
			this.target.UnregisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="evt"></param>
		private void PointerDownHandler(PointerDownEvent evt)
		{
			this.TargetStartPosition = this.Root.transform.position;
			this.PointerStartPosition = evt.position;
			this.target.CapturePointer(evt.pointerId);
			this.Enabled = true;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="evt"></param>
		private void PointerMoveHandler(PointerMoveEvent evt)
		{
			if (!this.Enabled || !this.target.HasPointerCapture(evt.pointerId))
				return;

			Vector3 pointerDelta = evt.position - this.PointerStartPosition;

			this.Root.transform.position = new Vector2(Mathf.Clamp(this.TargetStartPosition.x + pointerDelta.x, 0, this.Root.panel.visualTree.worldBound.width), Mathf.Clamp(this.TargetStartPosition.y + pointerDelta.y, 0, this.target.panel.visualTree.worldBound.height));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="evt"></param>
		private void PointerUpHandler(PointerUpEvent evt)
		{
			if (!this.Enabled || !this.target.HasPointerCapture(evt.pointerId))
				return;

			this.Enabled = false;
			this.target.ReleasePointer(evt.pointerId);
		}
	}
}
