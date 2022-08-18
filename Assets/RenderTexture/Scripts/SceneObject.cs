using TMPro;
using UnityEngine;

namespace Playground.RenderTarget
{
	/// <summary>
	/// The object in the scene that the user manipulates.
	/// </summary>
	public class SceneObject : MonoBehaviour
	{
		/// <summary>
		/// The text in the scene that is updated by the user.
		/// </summary>
		[SerializeField]
		[Tooltip("The text in the scene that is updated by the user.")]
		private TMP_Text text;

		/// <summary>
		/// The <see cref="MeshRenderer"/> reference whose material color is updated by the user.
		/// </summary>
		[SerializeField]
		[Tooltip("The MeshRenderer reference whose material color is updated by the user.")]
		private MeshRenderer cubeRenderer;

		/// <summary>
		/// Updates the text of <see cref="text"/>.
		/// </summary>
		/// <param name="value">The new value of <see cref="text"/>.</param>
		public void UpdateName(string value) => this.text.text = value;

		/// <summary>
		/// Updates the color of <see cref="cubeRenderer"/>'s material.
		/// </summary>
		/// <param name="value">The new material color of <see cref="cubeRenderer"/>.</param>
		public void UpdateColor(Color value) => this.cubeRenderer.material.color = value;

		/// <summary>
		/// Updates the y-axis rotation of <see cref="cubeRenderer"/>.
		/// </summary>
		/// <param name="value">The new y-rotation value of <see cref="cubeRenderer"/>'s transform.</param>
		public void UpdateRotation(float value) => this.cubeRenderer.transform.eulerAngles = Vector3.up * value;
	}
}
