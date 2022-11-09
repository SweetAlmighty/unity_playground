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
        private void Awake()
        {
            this.jumpAction.performed += player.Jump;
            this.lookAction.performed += context => player.Look(context.ReadValue<Vector2>());
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (this.moveAction.ReadValue<Vector3>() is Vector3 delta)
                this.player.Move(delta);
        }
    }
}
