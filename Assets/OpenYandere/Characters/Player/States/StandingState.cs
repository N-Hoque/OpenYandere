using OpenYandere.Characters.Player.States.Traits;
using OpenYandere.Managers;

namespace OpenYandere.Characters.Player.States
{
    internal class StandingState : IState
    {
        private PlayerManager m_playerManager;

        public void Constructor(PlayerManager playerManager)
        {
            m_playerManager = playerManager;
        }

        public void Enter()
        {
        }

        public MovementState HandleInput(InputData input)
        {
            // If the player is moving but not running, switch to the walking state.
            if(input.isMoving && !input.isRunning)
            {
                return MovementState.Walking;
            }

            // The player MUST be running, if they are moving but not walking.
            // If the player is moving and running, switch to running state.
            return input.isMoving ? MovementState.Running : MovementState.None;
        }

        public MovementState HandleUpdate(float deltaTime) =>
            // TODO: Stamina?
            MovementState.None;
    }
}