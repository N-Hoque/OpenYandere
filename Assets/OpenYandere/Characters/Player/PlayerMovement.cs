using System;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.Player
{
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class PlayerMovement : MonoBehaviour
    {
        private AnimatorData m_animatorData;

        private float m_cameraHorizontalAxis;

        private CharacterAnimator   m_characterAnimator;
        private CharacterController m_characterController;

        private InputData m_inputData;

        private float                m_movementSpeed;
        private MovementStateMachine m_movementStateMachine;

        [FormerlySerializedAs("WalkSpeed")] [Header("Movement Settings:")] [Tooltip("The walking speed of the player.")]
        public float walkSpeed = 2.0f;

        [FormerlySerializedAs("RunSpeed")] [Tooltip("The running speed of the player.")]
        public float runSpeed = 6.0f;

        private void Awake()
        {
            m_characterController = GetComponent<CharacterController>();
            m_characterAnimator   = GetComponent<CharacterAnimator>();
        }

        private void Update()
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis   = Input.GetAxis("Vertical");

            // Set the input data.
            m_inputData.isMoving  = true;
            m_inputData.isRunning = Input.GetKey(KeyCode.LeftShift);

            // If the player is moving on either axis.
            if(Math.Abs(horizontalAxis) > 0f || Math.Abs(verticalAxis) > 0f)
            {
                // Get the direction to move in.
                Vector3 moveDirection = new Vector3(horizontalAxis, 0, verticalAxis) * m_movementSpeed;
                moveDirection = Quaternion.AngleAxis(m_cameraHorizontalAxis, Vector3.up) * moveDirection;

                // Update the animator entry.
                m_animatorData.isRunning     = m_inputData.isRunning;
                m_animatorData.moveDirection = moveDirection;

                // Make the player look in the direction we are moving towards.
                transform.rotation = Quaternion.LookRotation(moveDirection);

                // Move in that direction.
                m_characterController.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                // Update the input data.
                m_inputData.isMoving  = false;
                m_inputData.isRunning = false;

                // Update the animator data.
                m_animatorData.isRunning     = false;
                m_animatorData.moveDirection = Vector3.zero;
            }

            // Update the character animator's data.
            m_characterAnimator.UpdateData(m_animatorData);

            // Have the current state handle the changes in input.
            m_movementStateMachine.HandleInput(m_inputData);

            // Update the current state.
            m_movementStateMachine.Update(Time.deltaTime);
        }

        public void Constructor(MovementStateMachine movementStateMachine) =>
            m_movementStateMachine = movementStateMachine;

        public void SetMovementSpeed(float speed) => m_movementSpeed = speed;

        public void SetCameraAxis(float horizontalAxis) => m_cameraHorizontalAxis = horizontalAxis;
    }
}