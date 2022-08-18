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
    public class CullingGroupSceneLogic : MonoBehaviour
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
        /// Reference to the <see cref="UIDocument"/> component used for this scene.
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to the UIDocument component used for this scene.")]
        private UIDocument uiDocument;

        /// <summary>
        /// The cached index of the current <see cref="Highlightable"/> that is closest to <see cref="player"/>.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Used to store the position of the player before moving it outside the range of
        /// the items being culled by <see cref="cullingGroup"/>.
        /// </summary>
        private Vector3 cachedPosition;

        /// <summary>
        /// Wrapper class instance used to communicate with the <see cref="CullingGroup"/> that handles the runtime logic.
        /// </summary>
        private CullingGroup<Highlightable> cullingGroup;

        /// <summary>
        /// Collection of <see cref="Highlightable"/> objects within the scene.
        /// </summary>
        private readonly List<Highlightable> highlightables = new();

        /// <summary>
        /// Unity Event used to set up <see cref="cullingGroup"/> and registers callbacks to the scene's UI.
        /// </summary>
        private void Start()
        {
            if (this.player == null)
                return;

            this.RegisterUICallbacks();
            this.InstantiateCullableItems();
            this.InitializeCullingGroup();
        }

        /// <summary>
        /// Cleans up <see cref="cullingGroup"/> when the scene is being exited.
        /// </summary>
        private void OnDestroy()
        {
            this.cullingGroup?.Destroy();
            this.cullingGroup = null;
        }

        /// <summary>
        /// Queries the <see cref="UIDocument"/> component for <see cref="VisualElement"/> instances
        /// and subscribes relevant registers callbacks.
        /// </summary>
        private void RegisterUICallbacks()
        {
            if (this.uiDocument.rootVisualElement.Q<RadioButtonGroup>() is { } radioButtonGroup)
                radioButtonGroup.RegisterValueChangedCallback(this.OnDistanceBandUpdated);

            if (this.uiDocument.rootVisualElement.Q<Button>() is { } button)
                button.RegisterCallback<ClickEvent>(_ => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        }

        /// <summary>
        /// Creates all of the objects that will be maintained through the use of <see cref="cullingGroup"/>.
        /// </summary>
        private void InstantiateCullableItems()
        {
            for (int i = 0; i < this.highlightableCount; i++)
                this.highlightables.Add(HighlightableGenerator.InstantiateItem());
        }

        /// <summary>
        /// Initializes <see cref="cullingGroup"/> with the player's <see cref="Transform"/> as its reference point, and the
        /// contents of <see cref="highlightables"/> as the items that will be "culled".
        /// </summary>
        private void InitializeCullingGroup()
        {
            this.cullingGroup = new(this.highlightables.Count, this.OnHighlightableStateChanged);
            this.cullingGroup.SetBoundingDistances(new[] { 2f, 4f, 8f, 16f, 32f });
            this.cullingGroup.SetDistanceReferencePoint(this.player.Transform);
            this.cullingGroup?.SetBoundingSpheres(this.highlightables.ToArray());
        }

        /// <summary>
        /// Callback for when the user changes the distance band used by <see cref="cullingGroup"/>.
        /// Updates <see cref="cullingGroup"/> and resets the states of the cullable items within the scene.
        /// </summary>
        /// <param name="changeEvent">Details regarding a value change event for a <see cref="VisualElement"/>.</param>
        private void OnDistanceBandUpdated(ChangeEvent<int> changeEvent)
        {
            this.distanceBand = changeEvent.newValue;
            this.cachedPosition = this.player.Transform.position;
            this.player.Transform.position = Vector3.one * 100;

            this.StartCoroutine(nameof(this.ResetPlayer));
        }

        /// <summary>
        /// Coroutine that resets the player's position in order to update the visibility state of
        /// the items within <see cref="cullingGroup"/>.
        /// </summary>
        /// <returns>An Enumerator that returns after the end of the current frame.</returns>
        private IEnumerator ResetPlayer()
        {
            yield return new WaitForEndOfFrame();
            this.player.Transform.position = this.cachedPosition;
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
