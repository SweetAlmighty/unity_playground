using UnityEngine;

namespace Playground.CullingGroup
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
        /// Container for horizontal and vertical axes used for player input.
        /// </summary>
        private Vector3 inputAxes;

        /// <summary>
        /// The multiplier used to move the player around the scene.
        /// </summary>
        private const float Speed = 25f;

        /// <summary>
        /// Maximum distance away from the scene's origin that the player is allowed to travel.
        /// </summary>
        private const float ClampDistance = 30f;

        /// <summary>
        /// Caches reference to the <see cref="UnityEngine.Transform"/> component attached to this object, and also sets
        /// <see cref="UnityEngine.Transform.hasChanged"/> for <see cref="Transform"/> to true, to trigger an initial run of
        /// the logic inside of <see cref="Update"/>.
        /// </summary>
        private void Start() => this.Transform = this.gameObject.transform;

        /// <summary>
        /// Unity Update event, used to move the player around the scene.
        /// </summary>
        private void Update()
        {
            this.inputAxes.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            this.inputAxes *= Speed * Time.deltaTime;

            if (Vector3.Distance(Vector3.zero, this.Transform.position + this.inputAxes) < ClampDistance)
                this.Transform.Translate(this.inputAxes);
        }
    }
}
