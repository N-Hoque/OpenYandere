using OpenYandere.Characters.Player;
using OpenYandere.Characters.Player.States;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private readonly MovementStateMachine m_movementStateMachine = new MovementStateMachine();

        [Header("References:")] [FormerlySerializedAs("Player")]
        public GameObject player;

        [FormerlySerializedAs("PlayerMovement")]
        public PlayerMovement playerMovement;

        private void Awake()
        {
            var standingState = new StandingState();
            var walkingState  = new WalkingState();
            var runningState  = new RunningState();

            standingState.Constructor(this);
            walkingState.Constructor(this);
            runningState.Constructor(this);

            m_movementStateMachine.RegisterState(MovementState.Standing, standingState);
            m_movementStateMachine.RegisterState(MovementState.Walking,  walkingState);
            m_movementStateMachine.RegisterState(MovementState.Running,  runningState);
            m_movementStateMachine.EnterState(MovementState.Standing);

            playerMovement.Constructor(m_movementStateMachine);
        }
    }
}