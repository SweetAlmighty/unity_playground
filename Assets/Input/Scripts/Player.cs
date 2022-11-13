using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Playground.InputManagement
{
    /// <summary>
    /// The player script, used for manipulated the user's view and position within the scene.
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The player's physical presence in the scene. Contains a collider and RigidBody for jumping logic.")]
        private GameObject character;

        [SerializeField]
        [Tooltip("The player's viewport in the scene.")]
        private new GameObject camera;

        [SerializeField]
        [Tooltip("The array of input components on the player. Used to swap between with components are being used to manipulate the player.")]
        private BaseInput[] baseInputs;

        [SerializeField]
        [Tooltip("The speed at which the player moves within the scene.")]
        private float moveSpeed = 10f;

        [SerializeField]
        [Tooltip("The speed at which the player rotates within the scene.")]
        private float rotationSpeed = 60f;

        /// <summary>
        /// The flag that is flipped when the player is jumping. When true, it prevents them from jumping again until they've hit the ground.
        /// </summary>
        private bool jumping;

        /// <summary>
        /// The value that is used to maintain the player's rotation in the scene.
        /// </summary>
        private Vector2 rotation;

        /// <summary>
        /// Flips the <see cref="jumping"/> flag back to false once the player has collised with the ground.
        /// </summary>
        /// <param name="collision">The entity that the player collided with.</param>
        private void OnCollisionEnter(Collision collision) => this.jumping = false;

        /// <summary>
        /// Updates which component is actively manipulating the player in the scene.
        /// </summary>
        /// <remarks>
        /// This should only be called by the UI.
        /// </remarks>
        /// <param name="selection">The selection the user made in the UI.</param>
        public void UpdateInputComponents(int selection)
        {
            for (int i = 0; i < baseInputs.Length; i++)
                baseInputs[i].enabled = i == selection;
        }

        /// <summary>
        /// Override for <see cref="Look(Vector2)"/> that takes in the context of the user's input.
        /// </summary>
        /// <param name="context">The context of the user's input that is triggering the look action.</param>
        public void Look(InputAction.CallbackContext context)
            => this.Look(context.ReadValue<Vector2>());

        /// <summary>
        /// Updates <see cref="rotation"/> based on the user's input, and sets the rotation value of <see cref="camera"/> to it.
        /// </summary>
        /// <param name="delta">The detla value retrieved from the user's input.</param>
        public void Look(Vector2 delta)
        {
            float rotateSpeed = this.rotationSpeed * Time.deltaTime;

            this.rotation.y += delta.x * rotateSpeed;
            this.rotation.x = Mathf.Clamp(this.rotation.x - delta.y * rotateSpeed, -89, 89);

            this.camera.transform.localEulerAngles = this.rotation;
        }

        /// <summary>
        /// Override for <see cref="Jump()"/> that takes in the context of the user's input.
        /// </summary>
        /// <param name="context">The context of the user's input that is triggering the jump.</param>
        public void Jump(InputAction.CallbackContext context)
        {
            if (context.interaction is TapInteraction)
                this.Jump();
        }

        /// <summary>
        /// Propels the player vertically, and flips <see cref="jumping"/> to true.
        /// </summary>
        public void Jump()
        {
            if (!this.jumping)
            {
                this.jumping = true;
                this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Updates the user's position within the scene.
        /// </summary>
        /// <param name="delta">The detla value retrieved from the user's input.</param>
        public void Move(Vector3 delta)
        {
            Vector3 move = Quaternion.Euler(0, this.camera.transform.eulerAngles.y, 0) * delta;
            this.transform.position += move * (Time.deltaTime * this.moveSpeed);
        }
    }
}