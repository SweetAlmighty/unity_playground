using Playground.RenderTarget;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.RenderTexture
{
	/// <summary>
	/// Manages logic for the RenderTexture scene.
	/// <remarks>
	/// Reference: https://github.com/needle-mirror/com.unity.ui/blob/9837807be0ff6bdf12df2e2bed0cbcef7936e3d9/Samples%7E/Runtime/Rendering/UITextureProjection.cs
	/// </remarks>
	/// </summary>
	public class RenderTextureSceneLogic : MonoBehaviour
	{
		/// <summary>
		/// The main camera within the scene.
		/// </summary>
		[SerializeField]
		[Tooltip("The main camera within the scene.")]
		private Camera mainCamera;

		/// <summary>
		/// The <see cref="UIDocument"/> instance that contains the <see cref="VisualElement"/> instances to connect to.
		/// </summary>
		[SerializeField]
		[Tooltip("The UIDocument instance that contains the VisualElement instances to connect to.")]
		private UIDocument uiDocument;

		/// <summary>
		/// The <see cref="GameObject"/> that is being manipulated by the user in the scene.
		/// </summary>
		[SerializeField]
		[Tooltip("The GameObject that is being manipulated by the user in the scene.")]
		private SceneObject sceneObject;

		/// <summary>
		/// The RenderTexture that is displaying the UI contained within <see cref="uiDocument"/>.
		/// </summary>
		private UnityEngine.RenderTexture renderTexture;

		/// <summary>
		/// An invalid screen space position returned by <see cref="ScreenCoordinatesToRenderTexture"/> if the user's
		/// input is within the bounds of the RenderTexture.
		/// </summary>
		private readonly Vector2 invalidPosition = new(float.NaN, float.NaN);

		/// <summary>
		/// Subscribes callbacks to the UI for manipulating <see cref="sceneObject"/>.
		/// </summary>
		private void Start()
		{
			this.renderTexture = this.uiDocument.panelSettings.targetTexture;

			this.uiDocument.panelSettings.SetScreenToPanelSpaceFunction(this.ScreenCoordinatesToRenderTexture);
			this.uiDocument.rootVisualElement.Q<TextField>().RegisterValueChangedCallback(this.UpdateSceneText);
			this.uiDocument.rootVisualElement.Q<Slider>().RegisterValueChangedCallback(this.UpdateObjectRotation);
			this.uiDocument.rootVisualElement.Q<RadioButtonGroup>().RegisterValueChangedCallback(this.UpdateObjectColor);

			_ = new DragDropManipulator(this.uiDocument.rootVisualElement.Q<VisualElement>("TitleBar"));
		}

		/// <summary>
		/// Callback for determining whether the user's cursor is within the bounds of the <see cref="renderTexture"/>.
		/// </summary>
		/// <param name="screenPosition">The screen space position of the user's cursor.</param>
		/// <returns>
		/// <see cref="invalidPosition"/> if the mouse cursor is outside of the bounds of <see cref="renderTexture"/>, and
		/// the transformed position of the mouse cursor from screen space to panel space.
		/// </returns>
		private Vector2 ScreenCoordinatesToRenderTexture(Vector2 screenPosition)
		{
			screenPosition.y = Screen.height - screenPosition.y;

			if (!Physics.Raycast(this.mainCamera.ScreenPointToRay(screenPosition), out RaycastHit hit))
				return this.invalidPosition;

			if (hit.transform.GetComponent<MeshRenderer>() is { } meshRenderer && meshRenderer.sharedMaterial.mainTexture != this.renderTexture)
				return this.invalidPosition;

			// Since y screen coordinates are usually inverted, we need to flip them.
			return new(hit.textureCoord.x * this.renderTexture.width, (1 - hit.textureCoord.y) * this.renderTexture.height);
		}

		/// <summary>
		/// Maps the <see cref="RadioButtonGroup"/> value to a <see cref="Color"/> option, used for
		/// updating <see cref="sceneObject"/>'s material color.
		/// </summary>
		/// <param name="value">The selected option of the <see cref="RadioButtonGroup"/> in the scene.</param>
		/// <returns>Red, Green, or Blue for 0, 1, and 2 respectively.</returns>
		private Color DetermineColor(int value) => value switch { 0 => Color.red, 1 => Color.green, 2 => Color.blue, _ => Color.white };

		/// <summary>
		/// Callback invoked when the <see cref="TextField"/> in the scene is updated.
		/// </summary>
		/// <param name="changeEvent">The event containing data related to the value being changed.</param>
		private void UpdateSceneText(ChangeEvent<string> changeEvent) => this.sceneObject.UpdateName(changeEvent.newValue);

		/// <summary>
		/// Callback invoked when the <see cref="Slider"/> in the scene is updated.
		/// </summary>
		/// <param name="changeEvent">The event containing data related to the value being changed.</param>
		private void UpdateObjectRotation(ChangeEvent<float> changeEvent) => this.sceneObject.UpdateRotation(changeEvent.newValue);

		/// <summary>
		/// Callback invoked when the <see cref="RadioButtonGroup"/> in the scene is updated.
		/// </summary>
		/// <param name="changeEvent">The event containing data related to the value being changed.</param>
		private void UpdateObjectColor(ChangeEvent<int> changeEvent) => this.sceneObject.UpdateColor(this.DetermineColor(changeEvent.newValue));
	}
}
