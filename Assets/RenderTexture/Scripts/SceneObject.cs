using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.RenderTarget
{
	/// <summary>
	///
	/// </summary>
	public class SceneObject : MonoBehaviour
	{
		/// <summary>
		///
		/// </summary>
		[SerializeField]
		[Tooltip("hjk")]
		private TMP_Text text;

		/// <summary>
		///
		/// </summary>
		[SerializeField]
		[Tooltip("hjk")]
		private MeshRenderer cube;

		/// <summary>
		///
		/// </summary>
		/// <param name="changeEvent"></param>
		public void UpdateName(ChangeEvent<string> changeEvent) => this.text.text = changeEvent.newValue;

		/// <summary>
		///
		/// </summary>
		/// <param name="changeEvent"></param>
		public void UpdateColor(ChangeEvent<int> changeEvent) => this.cube.material.color = changeEvent.newValue switch
		{
			0 => Color.red, 1 => Color.green, 2 => Color.blue, _ => Color.white
		};

		/// <summary>
		///
		/// </summary>
		/// <param name="changeEvent"></param>
		public void UpdateRotation(ChangeEvent<float> changeEvent) => this.cube.transform.eulerAngles = Vector3.up * changeEvent.newValue;
	}
}
