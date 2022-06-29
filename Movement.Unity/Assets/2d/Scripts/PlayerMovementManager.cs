using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2d
{
    public class PlayerMovementManager : MonoBehaviour
    {
        [Tooltip("The Player transform to move")]
        [SerializeField]
        private Transform Player;

        [Tooltip("The speed by which to move the player")]
        public float Speed = 5;

        /// <summary>
        /// TODO implement jump
        /// </summary>
        [Tooltip("The speed at which the player jumps")]
        public float JumpSpeed = 5;

        [SerializeField]
        private Animator playerAnimator;

        // Update is called once per frame
        void Update()
        {
            //a float value contained in [-1, 1]
            var horizontalInput = Input.GetAxis("Horizontal");
            var horizontalMovement = this.Speed * horizontalInput * Time.deltaTime;
            this.Player.position += this.Player.right * horizontalMovement;

            this.playerAnimator.SetBool("HasHorizontalInput", horizontalInput != 0);
            this.playerAnimator.SetFloat("HorizontalInput", horizontalInput);
        }
    }
}
