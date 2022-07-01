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

        private bool isMidJump = false;

        IEnumerator Jump()
        {
            isMidJump = true;
            var verticalVelocity = JumpSpeed;
            var gravity = -9.8f;
            var initialHeight = this.Player.transform.position.y;

            //Up
            yield return new WaitUntil(
                delegate ()
                {
                    this.Player.position +=
                        this.Player.transform.up * verticalVelocity * Time.deltaTime;
                    verticalVelocity += gravity * Time.deltaTime;
                    //Clamp vertical velocity to 0 if it becomes negative
                    if (verticalVelocity < 0)
                        verticalVelocity = 0;

                    this.playerAnimator.SetBool("IsJumping", verticalVelocity > 0);
                    return verticalVelocity == 0;
                }
            );


            //Down
            yield return new WaitUntil(
                delegate ()
                {
                    this.Player.position +=
                        this.Player.transform.up * verticalVelocity * Time.deltaTime;
                    verticalVelocity += gravity * Time.deltaTime;
                    var currentHeight = this.Player.position.y;
                    //Clamp player fall to the initial height
                    if (currentHeight < initialHeight)
                    {
                        this.Player.position = new Vector3(
                            this.Player.position.x,
                            initialHeight,
                            this.Player.position.z
                        );
                    }

                    this.playerAnimator.SetBool("IsFalling", Player.position.y > initialHeight);
                    return Player.position.y == initialHeight;
                }
            );
            isMidJump = false;
            yield return null;
        }

        /// <summary>
        /// If the player is jumping or falling, this animation should take precedence over the horizontal input
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            if (!this.isMidJump && Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(Jump());

            //a float value contained in [-1, 1]
            //has value 0 for no input
            var horizontalInput = Input.GetAxis("Horizontal");

            var horizontalMovement = this.Speed * horizontalInput * Time.deltaTime;
            this.Player.position += this.Player.right * horizontalMovement;

            this.playerAnimator.SetBool("HasHorizontalInput", horizontalInput != 0);
            this.playerAnimator.SetFloat("HorizontalInput", horizontalInput);
        }
    }
}
