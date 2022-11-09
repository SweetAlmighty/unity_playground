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
        private BaseInput[] baseInputs;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        protected float moveSpeed = 10f;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        protected float rotationSpeed = 60f;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public int selection;

        /// <summary>
        /// 
        /// </summary>
        private bool jumping;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 rotation;

        /// <summary>
        /// 
        /// </summary>
        protected Vector3 moveDelta;

        /// <summary>
        /// 
        /// </summary>
        protected Vector3 lookDelta;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision) => this.jumping = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selection"></param>
        public void UpdateInputComponents(int selection)
        {
            if (this.GetComponents<BaseInput>() is BaseInput[] components)
                for (int i = 0; i < components.Length; i++)
                    components[i].enabled = i == selection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Look(Vector2 value)
        {
            float rotateSpeed = this.rotationSpeed * Time.deltaTime;

            this.rotation.y += value.x * rotateSpeed;
            this.rotation.x = Mathf.Clamp(this.rotation.x - value.y * rotateSpeed, -89, 89);

            this.camera.transform.localEulerAngles = this.rotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Move(Vector3 value)
        {
            Vector3 move = Quaternion.Euler(0, this.camera.transform.eulerAngles.y, 0) * value;
            this.transform.position += move * (Time.deltaTime * this.moveSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Jump(InputAction.CallbackContext context)
        {
            if (!this.jumping && context.interaction is TapInteraction)
            {
                this.jumping = true;
                this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        }
    }
}