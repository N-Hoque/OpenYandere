using OpenYandere.Characters.Player.States.Traits;
using OpenYandere.Managers;

namespace OpenYandere.Characters.Player.States
{
    public class WalkingState : IState
    {
        private PlayerManager m_playerManager;

        public void Constructor(PlayerManager playerManager)
        {
            m_playerManager = playerManager;
        }

        public void Enter()
        {
            PlayerMovement playerMovement = m_playerManager.playerMovement;
            playerMovement.SetMovementSpeed(playerMovement.walkSpeed);
        }

        public MovementState HandleInput(InputData input)
        {
            // If the player is moving and running, switch to running state.
            if(input.isMoving && input.isRunning)
            {
                return MovementState.Running;
            }

            // If the player is not moving, switch to standing state.
            return !input.isMoving ? MovementState.Standing : MovementState.None;
        }

        public MovementState HandleUpdate(float deltaTime) => MovementState.None;
    }
}