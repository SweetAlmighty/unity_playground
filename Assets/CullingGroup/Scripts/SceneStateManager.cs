using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Virbela.CodingTest.Highlightables;

namespace Virbela.CodingTest.Utilities
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
        private Player player = null;
        
        /// <summary>
        /// Collection of <see cref="Highlightable"/> objects within the scene.
        /// </summary>
        [SerializeField]
        [Tooltip("Collection of Highlightable objects within the scene.")]
        private List<Highlightable> highlightables = Enumerable.Empty<Highlightable>().ToList();

        /// <summary>
        /// The cached index of the current <see cref="Highlightable"/> that is closest to <see cref="player"/>.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Wrapper class instance used to communicate with the <see cref="CullingGroup"/> that handles the runtime logic.
        /// </summary>
        private CullingGroupWrapper<Highlightable> cullingGroupWrapper;
        
        /// <summary>
        /// When <see cref="SceneStateManager"/> is enabled, <see cref="UpdateHighlighting"/> is subscribed to
        /// <see cref="Player.OnTransformUpdated"/>, and <see cref="cullingGroupWrapper"/> is initialized and set-up.
        /// </summary>
        private void Start()
        {
            if(this.player == null)
                return;

            this.player.OnTransformUpdated += this.UpdateHighlighting;
            this.cullingGroupWrapper = new CullingGroupWrapper<Highlightable>(this.highlightables.Count);
            this.cullingGroupWrapper.SetDistanceReferencePoint(this.player.Transform);
        }

        /// <summary>
        /// Catches key presses to add new <see cref="Highlightable"/> instances to the scene.
        /// </summary>
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
                this.AddHighlightableToCullingGroup(HighlightableGenerator.InstantiateBot());
            else if(Input.GetKeyDown(KeyCode.W))
                this.AddHighlightableToCullingGroup(HighlightableGenerator.InstantiateItem());
        }
        
        /// <summary>
        /// Cleans up <see cref="cullingGroupWrapper"/> when the application is being exited.
        /// </summary>
        private void OnApplicationQuit()
        {
            this.cullingGroupWrapper?.Destroy();
            this.cullingGroupWrapper = null;
        }

        /// <summary>
        /// Adds <paramref name="highlightable"/> to <see cref="highlightables"/>, and passes the updated list to
        /// <see cref="cullingGroupWrapper"/> to sync which objects in the scene are subjected to culling.
        /// </summary>
        /// <param name="highlightable">The new <see cref="Highlightable"/> instance to be tracked.</param>
        private void AddHighlightableToCullingGroup(Highlightable highlightable)
        {
            this.highlightables.Add(highlightable);
            this.cullingGroupWrapper?.SetBoundingSpheres(this.highlightables.ToArray());
            this.UpdateHighlighting();
        }

        /// <summary>
        /// Callback function that attempts to find which objects within <see cref="highlightables"/> are inside the
        /// acceptable range (determined by <see cref="cullingGroupWrapper"/>) and determines which of those is the closest
        /// to <see cref="player"/>.
        /// </summary>
        private void UpdateHighlighting()
        {
            this.highlightables[this.currentIndex].Unhighlight();
            int index = this.cullingGroupWrapper.FindClosestObjects().OrderBy(this.FindDistanceToPlayer).FirstOrDefault();
            this.highlightables[index].Highlight();
            this.currentIndex = index;
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
        private float FindDistanceToPlayer(int index) => (this.highlightables[index].Transform.position -
                                                          this.player.Transform.position).sqrMagnitude;
    }
}
