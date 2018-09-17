using System.Collections.Generic;

namespace GameLogic.State
{
    public class BaseState<T>
    {
        public T Owner; // renamed from "Target" to "Owner" by dxc, 2017/09/21
        private Dictionary<int, CallbackDelegate> _enterCallBackDic = new Dictionary<int, CallbackDelegate>();
        private Dictionary<int, CallbackDelegate> _exitCallBackDic = new Dictionary<int, CallbackDelegate>();
        private Dictionary<int, CallbackDelegate<float>> _runCallBackDic = new Dictionary<int, CallbackDelegate<float>>();
        private int _currentState;
        private int _nextState;
        private CallbackDelegate<float> _runCallBack;

        public virtual void Enter(T type)
        {
            _currentState = -1;
        }

        // Assume a state has only one related callback function
        public void AddEnterCallBack(int state, CallbackDelegate callBack)
        {
            if (!_enterCallBackDic.ContainsKey(state))
            {
                _enterCallBackDic.Add(state, callBack);
                //throw new System.Exception("dictionary already has key " + state);
                //return;
            }
        }

        public void RemoveEnterCallBack(int state)
        {
            _enterCallBackDic.Remove(state);
        }

        public void AddExitCallBack(int state, CallbackDelegate callBack)
        {
            if (!_exitCallBackDic.ContainsKey(state))
            {
                _exitCallBackDic.Add(state, callBack);
            }
        }

        public void RemoveExitCallBack(int state)
        {
            _exitCallBackDic.Remove(state);
        }

        public void AddRunCallBack(int state, CallbackDelegate<float> callBack)
        {
            if (!_runCallBackDic.ContainsKey(state))
            {
                _runCallBackDic.Add(state, callBack);
            }
        }

        public void RemoveRunCallBack(int state)
        {
            _runCallBackDic.Remove(state);
        }

        public void SwitchState(int state)
        {
            _nextState = state;
        }

        public virtual void Execute(T type, float timeSpan)
        {
            if (_nextState != _currentState)
            {
                if (_currentState != -1)
                {
                    if (_exitCallBackDic.ContainsKey(_currentState))
                    {
                        _exitCallBackDic[_currentState]();
                    }
                }

                _currentState = _nextState;

                if (_enterCallBackDic.ContainsKey(_currentState))
                {
                    _enterCallBackDic[_currentState]();
                }

                _runCallBack = null;

                if (_runCallBackDic.ContainsKey(_currentState))
                {
                    _runCallBack = _runCallBackDic[_currentState];
                }
            }
            if (_runCallBack == null)
            {
                return;
            }

            _runCallBack(timeSpan);
        }

        public virtual void Exit(T type)
        {
            if (_currentState != -1)
            {
                if (_exitCallBackDic.ContainsKey(_currentState))
                {
                    _exitCallBackDic[_currentState]();
                }
            }
        }

        public virtual void Pause(T type)
        {
        }

        public virtual void Resume(T type)
        {
        }

        protected void RemoveCallBacks(int state)
        {
            RemoveEnterCallBack(state);
            RemoveRunCallBack(state);
            RemoveExitCallBack(state);
        }
    }
}