using UnityEngine;

namespace Playground.InputManagement
{
    /// <summary>
    /// The base class for all input components within the Input demo.
    /// </summary>
    public abstract class BaseInput : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Reference to the player script, which is responsible for manipulating the player's perspective within the scene.")]
        protected Player player;
    }
}