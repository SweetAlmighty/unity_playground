using UnityEngine;
using UnityEngine.AI;

namespace Playground.NavMesh
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveToPoint : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Camera mainCamera;

        public void Start()
        {
            this.mainCamera = Camera.main;
            this.agent = this.gameObject.GetComponent<NavMeshAgent>();
        }

        public void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (Physics.Raycast(this.mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                this.agent.destination = hit.point;
        }
    }
}
