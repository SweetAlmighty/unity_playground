using UnityEngine;

namespace Playground.InputManagement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseInput : MonoBehaviour
    {
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
        protected Player player;

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
        public abstract Vector2 DetermineLookDelta();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Vector2 Look(Vector2 value)
        {
            float rotateSpeed = this.rotationSpeed * Time.deltaTime;

            this.rotation.y += value.x * rotateSpeed;
            this.rotation.x = Mathf.Clamp(this.rotation.x - value.y * rotateSpeed, -89, 89);

            return this.rotation;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract Vector3 DetermineMoveDelta();
    }
}