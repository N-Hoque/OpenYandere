using OpenYandere.Characters.Player.States.Traits;
using OpenYandere.Managers;

namespace OpenYandere.Characters.Player.States
{
    public class RunningState : IState
    {
        private PlayerManager m_playerManager;

        public void Constructor(PlayerManager playerManager)
        {
            m_playerManager = playerManager;
        }

        public void Enter()
        {
            PlayerMovement playerMovement = m_playerManager.playerMovement;
            playerMovement.SetMovementSpeed(playerMovement.runSpeed);
        }

        public MovementState HandleInput(InputData input)
        {
            // If the player is not running, but is moving switch to the walking state.
            if(!input.isRunning && input.isMoving)
            {
                return MovementState.Walking;
            }

            // If the player is not moving switch to the standing state.
            return !input.isMoving ? MovementState.Standing : MovementState.None;
        }

        public MovementState HandleUpdate(float deltaTime) => MovementState.None;
    }
}