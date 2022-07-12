using UnityEngine;
using Virbela.CodingTest.Utilities;

namespace Virbela.CodingTest.Highlightables
{
    /// <summary>
    /// Base class for objects that can be highlighted based on their distance to the <see cref="Player"/>.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class Highlightable : MonoBehaviour, ICullable
    {
        /// <summary>
        /// Instance of <see cref="HighlightableData"/> to be utilized by this instance.
        /// </summary>
        [SerializeField]
        [Tooltip("Instance of HighlightableData to be utilized by this instance.")]
        private HighlightableData data = null;

        /// <summary>
        /// Cached reference to the <see cref="UnityEngine.Bounds.extents"/> of the mesh contained within the
        /// <see cref="MeshFilter"/> component attached to this object.
        /// </summary>
        public Vector3 Extents { get; private set; }

        /// <inheritdoc cref="ICullable.Transform"/>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Cached reference to the <see cref="Renderer"/> component attached to this object.
        /// </summary>
        private Renderer cachedRenderer;

        /// <summary>
        /// When this <see cref="Highlightable"/> is loaded, references to the <see cref="Renderer"/> and
        /// <see cref="Transform"/> components will be cached, as well as the <see cref="Extents"/> of the object's mesh.
        /// </summary>
        private void Awake()
        {
            this.Transform = this.transform;
            this.cachedRenderer = this.GetComponent<Renderer>();

            MeshFilter filter = this.GetComponent<MeshFilter>();
            this.Extents = filter != null ? filter.mesh.bounds.extents : Vector3.zero;
        }

        /// <summary>
        /// Assigns the <see cref="Material.color"/> to the <see cref="HighlightableData.highlightColor"/>, indicating
        /// that it is the closest object to the <see cref="Player"/>.
        /// </summary>
        public void Highlight() => this.cachedRenderer.material.color = this.data.highlightColor;
        
        /// <summary>
        /// Assigns the <see cref="Material.color"/> to the <see cref="HighlightableData.baseColor"/>, indicating that it
        /// is <b>not</b> the closest object to the <see cref="Player"/>.
        /// </summary>
        public void Unhighlight() => this.cachedRenderer.material.color = this.data.baseColor;
    }
}
