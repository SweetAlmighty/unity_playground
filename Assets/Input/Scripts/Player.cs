using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Playground.InputManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private GameObject character;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private new GameObject camera;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        //private InputUsingActions input;
        private InputUsingActionAsset input;
        //private InputUsingStates input;

        /// <summary>
        /// 
        /// </summary>
        private bool jumping;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision) => this.jumping = false;

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (this.input.DetermineLookDelta() is Vector2 lookDelta && lookDelta.sqrMagnitude >= 0.01)
                this.camera.transform.localEulerAngles = lookDelta;

            if (this.input.DetermineMoveDelta() is Vector3 moveDelta && moveDelta != Vector3.zero)
            {
                // TODO: Tried abstracting this logic to BaseInput, but ran into issues where
                // player would only move on one axis, and ignored camera rotation.
                Vector3 move = Quaternion.Euler(0, this.camera.transform.eulerAngles.y, 0) * moveDelta;        
                this.transform.position += move * (Time.deltaTime * 10f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnJumpActionPerformed(InputAction.CallbackContext context)
        {
            if (!this.jumping && context.interaction is TapInteraction)
            {
                this.jumping = true;
                this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        }
    }
}