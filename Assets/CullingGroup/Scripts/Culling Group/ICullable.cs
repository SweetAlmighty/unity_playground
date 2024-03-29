using UnityEngine;

namespace Playground.CullingGroup
{
    /// <summary>
    /// Defines the necessary properties required by <see cref="CullingGroup{T}"/> in order to be culled.
    /// </summary>
    public interface ICullable
    {    
        /// <summary>
        /// Size information used to generate a reference <see cref="BoundingSphere"/>.
        /// </summary>
        Vector3 Extents { get; }

        /// <summary>
        /// Cached reference to a <see cref="UnityEngine.Transform"/>, used to get positional data within the scene.
        /// </summary>
        Transform Transform { get; }
    }
}
