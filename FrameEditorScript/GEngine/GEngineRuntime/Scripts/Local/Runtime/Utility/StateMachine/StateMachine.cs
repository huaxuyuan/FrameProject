using UnityEngine;
using System.Collections;
namespace  GameLogic.State
{
    public class StateMachine<T>
    {
        //StateMachine Owner
        private T m_Owner;

        private BaseState<T> m_CurrentState;
	    private BaseState<T> m_PreviousState;
        private BaseState<T> m_GlobalState;

        //Current State
        public BaseState<T> CurrentState
        {
            get
            {
                return m_CurrentState;
            }
            set
            {
                m_CurrentState = value;
            }

        }
        //Global State
        public BaseState<T> GlobalState
        {
            get
            {
                return m_GlobalState;
            }
        }
        //Previous State
        public BaseState<T> PreviousState
        {
            get
            {
                return m_PreviousState;
            }

        }
        //Init
        public StateMachine(T owner)
        {
            m_Owner = owner;
            m_CurrentState = null;
            m_PreviousState = null;
            m_GlobalState = null;
        }

        public void SetGlobalStateState(BaseState<T> GlobalState)
        {
            m_GlobalState = GlobalState;
            m_GlobalState.Owner = m_Owner;
            m_GlobalState.Enter(m_Owner);
        }

        public void SetCurrentState(BaseState<T> CurrentState)
        {

            if (CurrentState != m_CurrentState)
            {
                if (m_CurrentState != null)
                    m_CurrentState.Exit(m_Owner);

                m_CurrentState = CurrentState;
                if (CurrentState == null)
                    return;
                m_CurrentState.Owner = m_Owner;
                m_CurrentState.Enter(m_Owner);
            }
            //		else
            //		{
            //			m_CurrentState = null;
            //		}

        }

        public void StateMachineUpdate(float timeSpan)
        {

            if (m_GlobalState != null)
                m_GlobalState.Execute(m_Owner, timeSpan);

            if (m_CurrentState != null)
                m_CurrentState.Execute(m_Owner, timeSpan);
        }

        public void ChangeState(BaseState<T> newState)
        {
            if (newState == null)
            {
                Debug.LogError("the state is not exist...");
            }

            m_CurrentState.Exit(m_Owner);

            m_PreviousState = m_CurrentState;


            m_CurrentState = newState;

            m_CurrentState.Owner = m_Owner;

            m_CurrentState.Enter(m_Owner);
        }
        //Revert State
        public void RevertToPreviousState()
        {
            ChangeState(m_PreviousState);

        }
    }
}
