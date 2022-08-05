using UnityEngine;

namespace Playground.CullingGroup
{
    /// <summary>
    /// Singleton responsible for generating <see cref="Highlightable"/> instances during runtime.
    /// </summary>
    public class HighlightableGenerator : MonoBehaviour
    {
        /// <summary>
        /// The distance from the origin that 'Item' prefabs will be instantiated within.
        /// </summary>
        [SerializeField]
        [Tooltip("The distance from the origin that 'Item' prefabs will be instantiated within.")]
        private int radius = 30;

        /// <summary>
        /// <see cref="Highlightable"/> prefab utilizing to the "Item" configuration of <see cref="HighlightableData"/>.
        /// </summary>
        [SerializeField]
        [Tooltip("Highlightable prefab utilizing to the 'Item' configuration of HighlightableData.")]
        private Highlightable item;

        /// <summary>
        /// The in-scene object to place newly generated <see cref="item"/> instances under.
        /// </summary>
        [SerializeField]
        [Tooltip("The in-scene object to place newly generated item instances under.")]
        private Transform itemsParent;

        /// <summary>
        /// Static instance used to instantiate <see cref="Highlightable"/>s.
        /// </summary>
        private static HighlightableGenerator Instance { get; set; }

        /// <summary>
        /// Stores references to parent objects in the scene, and stores the in-scene script as a static reference for use
        /// during the application's runtime.
        /// </summary>
        private void Awake() => Instance = this;

        /// <summary>
        /// Creates an instance of <see cref="item"/> within the scene.
        /// </summary>
        /// <returns>The newly instantiated <see cref="Highlightable"/>.</returns>
        public static Highlightable InstantiateItem() => Instance.Instantiate(Instance.item, Instance.itemsParent);

        /// <summary>
        /// Creates a new instance of a <see cref="Highlightable"/> prefab at a random position within the scene.
        /// </summary>
        /// <param name="original">The prefab that is being used to instantiate the new object.</param>
        /// <param name="parent">The object within the scene that the newly instantiated object while be parented to.</param>
        /// <returns>The newly instantiated <see cref="Highlightable"/>.</returns>
        private Highlightable Instantiate(Highlightable original, Transform parent)
        {
            Vector2 position = Random.insideUnitCircle * this.radius;
            return GameObject.Instantiate(original, new(position.x, 0, position.y), Quaternion.identity, parent);
        }
    }
}
