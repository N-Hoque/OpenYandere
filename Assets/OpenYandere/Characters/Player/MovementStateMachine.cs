using System.Collections.Generic;

using OpenYandere.Characters.Player.States.Traits;

namespace OpenYandere.Characters.Player
{
    public enum MovementState
    {
        None,
        Standing,
        Walking,
        Running
    }

    public class MovementStateMachine
    {
        private readonly Dictionary<MovementState, IState> m_registeredStates = new Dictionary<MovementState, IState>();

        private IState m_currentState;

        public void RegisterState(MovementState stateName, IState state)
        {
            m_registeredStates.Add(stateName, state);
        }

        public void EnterState(MovementState stateName)
        {
            // If the state is not registered, return.
            if(!m_registeredStates.ContainsKey(stateName))
            {
                return;
            }

            // Update the current state and call the enter method.
            m_currentState = m_registeredStates[stateName];
            m_currentState.Enter();
        }

        public void HandleInput(InputData input)
        {
            // Let the current state, handle the input.
            MovementState stateName = m_currentState.HandleInput(input);

            // Switch states, if the state is not equal to none.
            if(stateName != MovementState.None)
            {
                EnterState(stateName);
            }
        }

        public void Update(float deltaTime)
        {
            // Let the current state, handle update.
            MovementState stateName = m_currentState.HandleUpdate(deltaTime);

            // Switch states, if the state is not equal to none.
            if(stateName != MovementState.None)
            {
                EnterState(stateName);
            }
        }
    }
}