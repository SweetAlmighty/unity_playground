using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.InputManagement
{
	/// <summary>
	/// The UI Component used for the "Input" scene.
	/// </summary>
	public class InputUI : MonoBehaviour
	{
		/// <summary>
		/// Reference to the <see cref="UIDocument"/> component used for this scene.
		/// </summary>
		[SerializeField]
		[Tooltip("Reference to the UIDocument component used for this scene.")]
		private UIDocument uiDocument;

        /// <summary>
		/// 
        /// </summary>
        [SerializeField]
        [Tooltip("")]
        private Player player;

        /// <summary>
        /// Unity Event function that subscribes scene-loading callbacks to UI buttons when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
		{
			if (this.uiDocument.rootVisualElement.Q<Button>() is { } button)
				button.RegisterCallback<ClickEvent>(_ => UnityEngine.SceneManagement.SceneManager.LoadScene(0));

            if (this.uiDocument.rootVisualElement.Q<RadioButtonGroup>() is { } radioGroup)
                _ = radioGroup.RegisterValueChangedCallback(evt => player.UpdateInputComponents(evt.newValue));
		}
	}
}
