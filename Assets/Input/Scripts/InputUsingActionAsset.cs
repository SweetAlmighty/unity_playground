using UnityEngine;

namespace Playground.InputManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class InputUsingActionAsset : BaseInput
    {
        /// <summary>
        /// 
        /// </summary>
        private ActionAsset controls;

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() => this.controls.Enable();

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() => this.controls.Disable();

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            this.controls = new ActionAsset();
            this.controls.gameplay.Jump.performed += player.Jump;
            this.controls.gameplay.Look.performed += context => player.Look(context.ReadValue<Vector2>());
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (this.controls.gameplay.Move.ReadValue<Vector3>() is Vector3 delta)
                this.player.Move(delta);
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        public override Vector2 DetermineLookDelta() =>
            this.Look(this.controls.gameplay.Look.ReadValue<Vector2>());

        /// <summary>
        /// 
        /// </summary>
        public override Vector3 DetermineMoveDelta() =>
            this.Move(this.controls.gameplay.Move.ReadValue<Vector3>());
        */
    }
}
