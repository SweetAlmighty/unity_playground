using System;
using UnityEngine;
using Virbela.CodingTest.Highlightables;

namespace Virbela.CodingTest
{
    /// <summary>
    /// Object used as point-of-reference when calculating distances to determine the "closest" <see cref="Highlightable"/>
    /// in the scene. 
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Instance of <see cref="UnityEngine.Transform"/> to be utilized by this class.
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Event that is fired whenever <see cref="Transform"/> has been updated.
        /// </summary>
        public event Action OnTransformUpdated;
        
        /// <summary>
        /// Caches reference to the <see cref="UnityEngine.Transform"/> component attached to this object, and also sets
        /// <see cref="UnityEngine.Transform.hasChanged"/> for <see cref="Transform"/> to true, to trigger an initial run of
        /// the logic inside of <see cref="Update"/>.
        /// </summary>
        private void Start()
        {
            this.Transform = this.gameObject.transform;
            this.Transform.hasChanged = true;
        }

        /// <summary>
        /// Checks every frame to see if <see cref="UnityEngine.Transform.hasChanged"/> for <see cref="Transform"/> has been
        /// flagged as true, and will invoke<see cref="Player.OnTransformUpdated"/> in the event that that it has been.
        /// </summary>
        /// <remarks>
        /// I'd gone back and forth on whether or not FixedUpdate would be appropriate here, but as
        /// <see cref="CullingGroup"/> doesn't seem to use physics for its calculations, I opted to stay with
        /// <see cref="Update"/>.
        /// </remarks>
        private void Update()
        {
            if (!this.Transform.hasChanged)
                return;
            
            this.OnTransformUpdated?.Invoke();
            this.Transform.hasChanged = false;
        }
    }
}

