using System;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcMovement : MonoBehaviour
    {
        private AnimatorData m_animatorData;

        public NavMeshAgent NavigationAgent => navMeshAgent;

        [FormerlySerializedAs("_characterAnimator")] [Header("References:")] [SerializeField]
        private CharacterAnimator characterAnimator;

        [FormerlySerializedAs("_navMeshAgent")] [SerializeField]
        private NavMeshAgent navMeshAgent;

        [Header("Settings:")] 
        [FormerlySerializedAs("WalkSpeed")] [Tooltip("The walking speed of the NPC.")]
        public float walkSpeed = 2.0f;

        [FormerlySerializedAs("RunSpeed")] [Tooltip("The running speed of the NPC.")]
        public float runSpeed = 6.0f;

        [FormerlySerializedAs("IsRunning")] [Tooltip("Is the NPC running?")]
        public bool isRunning;

        private void Awake()
        {
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;
        }

        private void Update()
        {
            // Get the current velocity of the navigation agent.
            var horizontalAxis = navMeshAgent.velocity.x;
            var verticalAxis   = navMeshAgent.velocity.z;

            // Check if either axis is more than zero - meaning the NPC is moving.
            if(Math.Abs(horizontalAxis) > 0f || Math.Abs(verticalAxis) > 0f)
            {
                // Get the speed to move at.
                var movementSpeed = isRunning ? runSpeed : walkSpeed;

                // Calculate the direction to move in.
                var moveDirection = new Vector3(horizontalAxis, 0, verticalAxis);

                // Update the speed of the navigation agent.
                navMeshAgent.speed = movementSpeed;

                // Update the animator entry.
                m_animatorData.isRunning     = isRunning;
                m_animatorData.moveDirection = moveDirection;
            }
            else
            {
                // Update the animator data.
                m_animatorData.isRunning     = false;
                m_animatorData.moveDirection = Vector3.zero;
            }

            // Update the data the character animator is using.
            characterAnimator.UpdateData(m_animatorData);
        }

        public void Resume()
        {
            // Resume navigation agent.
            navMeshAgent.isStopped = false;

            // Resume the animator.
            characterAnimator.Resume();
        }

        public void Pause()
        {
            // Pause the navigation agent.
            navMeshAgent.isStopped = true;

            // Pause the animator.
            characterAnimator.Pause();
        }
    }
}