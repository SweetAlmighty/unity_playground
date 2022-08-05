using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.CullingGroup
{
    /// <summary>
    /// Manager class used to manage <see cref="UnityEngine.CullingGroup"/> updates.
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// In-scene <see cref="Player"/> reference used for distances calculations.
        /// </summary>
        [SerializeField]
        [Tooltip("In-scene Player reference used for distances calculations.")]
        private Player player;

        /// <summary>
        /// The distance band index being used to by <see cref="CullingGroup"/> to show or hide objects within the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("The distance band index being used to by CullingGroup to show or hide objects within the scene.")]
        private int distanceBand;

        /// <summary>
        /// The amount of <see cref="Highlightable"/> instances to create within the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("The amount of Highlightable instances to create within the scene.")]
        private int highlightableCount;

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
        /// When <see cref="SceneManager"/> is enabled, <see cref="UpdateHighlighting"/> is subscribed to
        /// <see cref="Player.OnTransformUpdated"/>, and <see cref="cullingGroup"/> is initialized and set-up.
        /// </summary>
        private void Start()
        {
            if (this.player == null)
                return;

            for (int i = 0; i < this.highlightableCount; i++)
                this.highlightables.Add(HighlightableGenerator.InstantiateItem());

            this.cullingGroup = new(this.highlightables.Count, this.OnHighlightableStateChanged);
            this.cullingGroup.SetBoundingDistances(new[] { 2f, 4f, 8f, 16f, 32f });
            this.cullingGroup.SetDistanceReferencePoint(this.player.Transform);
            this.cullingGroup?.SetBoundingSpheres(this.highlightables.ToArray());
        }

        /// <summary>
        ///
        /// </summary>
        private void OnEnable()
        {
            if(this.GetComponent<UIDocument>() is not { } uiDocument)
                return;

            if(uiDocument.rootVisualElement.Q<RadioButtonGroup>() is { } radioButtonGroup)
                radioButtonGroup.RegisterValueChangedCallback(this.OnDistanceBandUpdated);
        }

        /// <summary>
        ///
        /// </summary>
        private Vector3 test;

        /// <summary>
        ///
        /// </summary>
        /// <param name="changeEvent"></param>
        private void OnDistanceBandUpdated(ChangeEvent<int> changeEvent)
        {
            this.distanceBand = changeEvent.newValue;
            this.test = this.player.Transform.position;
            this.player.Transform.position = Vector3.one * 100;

            this.StartCoroutine(nameof(this.ResetPlayer));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetPlayer()
        {
            yield return new WaitForEndOfFrame();
            this.player.Transform.position = this.test;
        }

        /// <summary>
        /// Callback invoked on every <see cref="BoundingSphere"/> being monitored by the <see cref="CullingGroup"/> that has changed states.
        /// </summary>
        /// <param name="groupEvent">
        /// The information regarding the previous and current state of the <see cref="BoundingSphere"/> that has been updated.
        /// </param>
        private void OnHighlightableStateChanged(CullingGroupEvent groupEvent)
        {
            this.highlightables[groupEvent.index].gameObject.SetActive(groupEvent.currentDistance == this.distanceBand);
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
            this.currentIndex = this.FindClosestObjects(this.distanceBand)
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
        /// <param name="startingIndex">The index of the <see cref="BoundingSphere"/> to begin searching at.</param>
        /// <returns>
        /// A collection of indices that represent the objects within the internal <see cref="UnityEngine.CullingGroup"/>'s
        /// distance band.
        /// </returns>
        public IEnumerable<int> FindClosestObjects(int distanceIndex = 0, int startingIndex = 0) =>
            this.cullingGroup.QueryIndices(true, distanceIndex, startingIndex, out int count)[..count];
    }
}
