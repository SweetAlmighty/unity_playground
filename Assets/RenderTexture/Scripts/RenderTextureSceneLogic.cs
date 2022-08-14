using Playground.RenderTarget;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.RenderTexture
{
	/// <summary>
	/// https://github.com/needle-mirror/com.unity.ui/blob/9837807be0ff6bdf12df2e2bed0cbcef7936e3d9/Samples%7E/Runtime/Rendering/UITextureProjection.cs
	/// </summary>
	public class RenderTextureSceneLogic : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("hjk")]
		private Camera main;

		[SerializeField]
		[Tooltip("hjk")]
		private SceneObject obj;

		[SerializeField]
		[Tooltip("hjk")]
		private UIDocument uiDocument;

		/// <summary>
		///
		/// </summary>
		private UnityEngine.RenderTexture targetTexture;

		/// <summary>
		///
		/// </summary>
		private readonly Vector2 invalidPosition = new(float.NaN, float.NaN);

		/// <summary>
		///
		/// </summary>
		private void Start()
		{
			this.targetTexture = this.uiDocument.panelSettings.targetTexture;

			this.uiDocument.panelSettings.SetScreenToPanelSpaceFunction(this.ScreenCoordinatesToRenderTexture);
			this.uiDocument.rootVisualElement.Q<TextField>().RegisterValueChangedCallback(this.obj.UpdateName);
			this.uiDocument.rootVisualElement.Q<Slider>().RegisterValueChangedCallback(this.obj.UpdateRotation);
			this.uiDocument.rootVisualElement.Q<RadioButtonGroup>().RegisterValueChangedCallback(this.obj.UpdateColor);

			_ = new DragDropManipulator(this.uiDocument.rootVisualElement.Q<VisualElement>("TitleBar"));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <returns></returns>
		private Vector2 ScreenCoordinatesToRenderTexture(Vector2 screenPosition)
		{
			screenPosition.y = Screen.height - screenPosition.y;

			if (!Physics.Raycast(this.main.ScreenPointToRay(screenPosition), out RaycastHit hit))
				return this.invalidPosition;

			if (hit.transform.GetComponent<MeshRenderer>() is { } meshRenderer && meshRenderer.sharedMaterial.mainTexture != this.targetTexture)
				return this.invalidPosition;

			// Since y screen coordinates are usually inverted, we need to flip them.
			return new(hit.textureCoord.x * this.targetTexture.width, (1 - hit.textureCoord.y) * this.targetTexture.height);
		}
	}
}
