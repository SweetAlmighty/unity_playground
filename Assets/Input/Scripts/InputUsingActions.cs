using UnityEngine;
using UnityEngine.InputSystem;

namespace Playground.InputManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class InputUsingActions : BaseInput
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private InputAction moveAction;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private InputAction lookAction;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private InputAction jumpAction;

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            this.moveAction.Enable();
            this.lookAction.Enable();
            this.jumpAction.Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            this.moveAction.Disable();
            this.lookAction.Disable();
            this.jumpAction.Disable();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Awake() => this.jumpAction.performed += player.OnJumpActionPerformed;

        /// <summary>
        /// 
        /// </summary>
        public override Vector2 DetermineLookDelta() => this.Look(this.lookAction.ReadValue<Vector2>());

        /// <summary>
        /// 
        /// </summary>
        public override Vector3 DetermineMoveDelta() => this.moveAction.ReadValue<Vector3>();
    }
}
