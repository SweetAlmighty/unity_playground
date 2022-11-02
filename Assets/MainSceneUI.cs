using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Playground
{
	/// <summary>
	/// The UI Component used for the main scene.
	/// </summary>
	public class MainSceneUI : MonoBehaviour
	{
		/// <summary>
		/// Reference to the <see cref="UIDocument"/> component used for this scene.
		/// </summary>
		[SerializeField]
		[Tooltip("Reference to the UIDocument component used for this scene.")]
		private UIDocument uiDocument;

		/// <summary>
		/// Unity Event function that subscribes scene-loading callbacks to UI buttons when the object becomes enabled and active.
		/// </summary>
		private void OnEnable()
		{
			this.uiDocument.rootVisualElement.Q<Button>("Input").RegisterCallback<ClickEvent>(this.OnInputClicked);
			this.uiDocument.rootVisualElement.Q<Button>("NavMesh").RegisterCallback<ClickEvent>(this.OnNavMeshClicked);
			this.uiDocument.rootVisualElement.Q<Button>("CullingGroup").RegisterCallback<ClickEvent>(this.OnCullingGroupClicked);
			this.uiDocument.rootVisualElement.Q<Button>("RenderTexture").RegisterCallback<ClickEvent>(this.OnRenderTextureClicked);
		}

		/// <summary>
		/// Callback that loads the 'NavMesh' scene.
		/// </summary>
		/// <param name="clickEvent">Event details pertaining to a left-mouse click on a <see cref="UnityEngine.UIElements"/>.</param>
		private void OnNavMeshClicked(ClickEvent clickEvent) => SceneManager.LoadScene(1);

		/// <summary>
		/// Callback that loads the 'CullingGroup' scene.
		/// </summary>
		/// <param name="clickEvent">Event details pertaining to a left-mouse click on a <see cref="UnityEngine.UIElements"/>.</param>
		private void OnCullingGroupClicked(ClickEvent clickEvent) => SceneManager.LoadScene(2);

		/// <summary>
		/// Callback that loads the 'RenderTexture' scene.
		/// </summary>
		/// <param name="clickEvent">Event details pertaining to a left-mouse click on a <see cref="UnityEngine.UIElements"/>.</param>
		private void OnRenderTextureClicked(ClickEvent clickEvent) => SceneManager.LoadScene(3);

		/// <summary>
		/// Callback that loads the 'Input' scene.
		/// </summary>
		/// <param name="clickEvent">Event details pertaining to a left-mouse click on a <see cref="UnityEngine.UIElements"/>.</param>
		private void OnInputClicked(ClickEvent clickEvent) => SceneManager.LoadScene(4);
	}
}
