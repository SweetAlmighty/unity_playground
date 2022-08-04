using System.Collections.Generic;
using System.Linq;
using Playground.CullingGroup.Highlightables;
using UnityEngine;

namespace Playground.CullingGroup
{
    /// <summary>
    /// Manager class used to manage <see cref="UnityEngine.CullingGroup"/> updates.
    /// </summary>
    public class SceneStateManager : MonoBehaviour
    {
        /// <summary>
        /// In-scene <see cref="Player"/> reference used for distances calculations.
        /// </summary>
        [SerializeField]
        [Tooltip("In-scene Player reference used for distances calculations.")]
        private Player player;

        /// <summary>
        /// The amount of <see cref="Highlightable"/> instances to create within the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("The amount of Highlightable instances to create within the scene.")]
        private int highlightableCount;
        
        /// <summary>
        /// The distance band index being used to by <see cref="CullingGroup"/> to show or hide objects within the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("The distance band index being used to by CullingGroup to show or hide objects within the scene.")]
        [Range(0, 4)]
        private int distanceIndex;

        /// <summary>
        /// The cached index of the current <see cref="Highlightable"/> that is closest to <see cref="player"/>.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Wrapper class instance used to communicate with the <see cref="CullingGroup"/> that handles the runtime logic.
        /// </summary>
        private CullingGroup<Highlightable> cullingGroup;
        
        /// <summary>
        /// Collection of <see cref="Highlightable"/> objects within the scene.
        /// </summary>
        private readonly List<Highlightable> highlightables = new();

        /// <summary>
        /// When <see cref="SceneStateManager"/> is enabled, <see cref="UpdateHighlighting"/> is subscribed to
        /// <see cref="Player.OnTransformUpdated"/>, and <see cref="cullingGroup"/> is initialized and set-up.
        /// </summary>
        private void Start()
        {
            if (this.player == null)
                return;

            for (int i = 0; i < this.highlightableCount; i++)
                this.highlightables.Add(HighlightableGenerator.InstantiateItem());

            this.cullingGroup = new(this.highlightables.Count, this.OnHighlightableStateChanged);

            this.cullingGroup.SetDistanceReferencePoint(this.player.Transform);
            this.cullingGroup?.SetBoundingSpheres(this.highlightables.ToArray());
        }

        /// <summary>
        /// Callback invoked on every <see cref="BoundingSphere"/> being monitored by the <see cref="CullingGroup"/> that has changed states.
        /// </summary>
        /// <param name="groupEvent">
        /// The information regarding the previous and current state of the <see cref="BoundingSphere"/> that has been updated.
        /// </param>
        private void OnHighlightableStateChanged(CullingGroupEvent groupEvent)
        {
            this.highlightables[groupEvent.index].gameObject.SetActive(groupEvent.currentDistance == this.distanceIndex);
            this.UpdateHighlighting();
        }

        /// <summary>
        /// Cleans up <see cref="cullingGroup"/> when the application is being exited.
        /// </summary>
        private void OnApplicationQuit()
        {
            this.cullingGroup?.Destroy();
            this.cullingGroup = null;
        }

        /// <summary>
        /// Callback function that attempts to find which objects within <see cref="highlightables"/> are inside the
        /// acceptable range (determined by <see cref="cullingGroup"/>) and determines which of those is the closest
        /// to <see cref="player"/>.
        /// </summary>
        private void UpdateHighlighting()
        {
            this.highlightables[this.currentIndex].Unhighlight();
            this.currentIndex = this.FindClosestObjects(this.distanceIndex)
                                    .OrderBy(this.FindDistanceToPlayer)
                                    .FirstOrDefault();
            this.highlightables[this.currentIndex].Highlight();
        }

        /// <summary>
        /// Helper function used to calculate the distance values for all objects determined to be within an acceptable
        /// range of the <see cref="Player"/>.
        /// </summary>
        /// <param name="index">
        /// The position of the object within the <see cref="highlightables"/> array whose distance from the
        /// <see cref="Player"/> is being calculated.
        /// </param>
        /// <returns>
        /// The distance between the <see cref="Highlightable"/> object, indicated by <paramref name="index"/>,
        /// and the <see cref="Player"/>.
        /// </returns>
        private float FindDistanceToPlayer(int index) =>
            (this.highlightables[index].Transform.position - this.player.Transform.position).sqrMagnitude;
        
        /// <summary>
        /// Queries the internal <see cref="UnityEngine.CullingGroup"/> to find the positions of objects within the scene
        /// that are within the distance band indicated by <paramref name="distanceIndex"/>.
        /// </summary>
        /// <param name="distanceIndex">The distance band that the retrieved <see cref="BoundingSphere"/> must be in.</param>
        /// <param name="firstIndex">The index of the <see cref="BoundingSphere"/> to begin searching at.</param>
        /// <returns>
        /// A collection of indices that represent the objects within the internal <see cref="UnityEngine.CullingGroup"/>'s
        /// distance band.
        /// </returns>
        public IEnumerable<int> FindClosestObjects(int distanceIndex = 0, int firstIndex = 0) =>
            this.cullingGroup.QueryIndices(true, distanceIndex, firstIndex, out int count)[..count];
    }
}
