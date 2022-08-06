using UnityEngine;
using UnityEngine.AI;

namespace Playground.NavMesh
{
    /// <summary>
    /// Manages logic for updating the main <see cref="NavMeshAgent"/>'s destination.
    /// </summary>
    public class NavMeshSceneLogic : MonoBehaviour
    {
        /// <summary>
        /// The camera that will be used to Raycast from.
        /// </summary>
        [SerializeField]
        [Tooltip("The camera that will be used to Raycast from.")]
        private Camera mainCamera;

        /// <summary>
        /// The agent whose destination will be changed as a result of a successful Raycast.
        /// </summary>
        [SerializeField]
        [Tooltip("")]
        private NavMeshAgent agent;

        /// <summary>
        /// Unity Event function used to check for user input.
        /// </summary>
        public void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (Physics.Raycast(this.mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                this.agent.destination = hit.point;
        }
    }
}
