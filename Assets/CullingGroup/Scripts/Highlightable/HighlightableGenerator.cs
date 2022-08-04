using UnityEngine;

namespace Playground.CullingGroup.Highlightables
{
    /// <summary>
    /// Singleton responsible for generating <see cref="Highlightable"/> instances during runtime.
    /// </summary>
    public class HighlightableGenerator : MonoBehaviour
    {
        /// <summary>
        /// <see cref="Highlightable"/> prefab utilizing to the "Item" configuration of <see cref="HighlightableData"/>.
        /// </summary>
        [SerializeField]
        [Tooltip("Highlightable prefab utilizing to the 'Item' configuration of HighlightableData.")]
        private Highlightable item;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        [Tooltip("Highlightable prefab utilizing to the 'Item' configuration of HighlightableData.")]
        private int radius = 30;

        /// <summary>
        /// The in-scene object to place newly generated <see cref="item"/> instances under.
        /// </summary>
        private Transform itemsParent;

        /// <summary>
        /// Static instance used to instantiate <see cref="Highlightable"/>s.
        /// </summary>
        private static HighlightableGenerator Instance { get; set; }

        /// <summary>
        /// Stores references to parent objects in the scene, and stores the in-scene script as a static reference for use
        /// during the application's runtime.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            this.itemsParent = this.FindParent("Items").transform;
        }

        /// <summary>
        /// Helper function that finds an object within the scene to parent newly generated <see cref="Highlightable"/>s
        /// under, and will generate a new parent object if one wasn't in the scene when the application started.
        /// </summary>
        /// <param name="parentName">The name of the parent object being searched for.</param>
        /// <returns>The found parent object, pre-existing or otherwise.</returns>
        private GameObject FindParent(string parentName) => GameObject.Find(parentName) ?? new(parentName);

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
