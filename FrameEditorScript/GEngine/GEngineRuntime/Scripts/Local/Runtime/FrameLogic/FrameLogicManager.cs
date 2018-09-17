using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameLogicData;
using ConfigData;
using GameLogic.State;
using Runtime.FrameLogic.State;
using UnityEngine;
using ConfigData;
namespace Runtime.FrameLogic
{
    public class FrameLogicManager : SingletonNotMono<FrameLogicManager>
    {
        public enum APPSTATE
        {
            APP_STATE_INITIALIZE = 1,// initialize game resource
            APP_STATE_EDIT,
        };
        private StateMachine<FrameLogicManager> _gameState;                       //游戏状态机
        private Dictionary<APPSTATE, BaseState<FrameLogicManager>> _gameStateArray;   //各游戏状态实例Map
        private Vector2 _scrollBar;
        public void InitializeFrame()
        {
            _gameState = new StateMachine<FrameLogicManager>(this);
            _gameStateArray = new Dictionary<APPSTATE, BaseState<FrameLogicManager>>();
            _gameStateArray.Add(APPSTATE.APP_STATE_INITIALIZE, new FrameInitializeState());
            _gameStateArray.Add(APPSTATE.APP_STATE_EDIT, new FrameEditState());
            SetAppState(APPSTATE.APP_STATE_INITIALIZE);
            //
        }
        public void SetAppState(APPSTATE state)
        {
            _gameState.SetCurrentState(_gameStateArray[state]);
        }
        public void SelectFrame(FrameConfigData configData)
        {
            VoConfigDataManager.Instance.SelectFrameConfigData(configData);
            FrameDispatchLogicManager.Instance.InitializeFrameLogic();
        }
        public void ReleaseFrameLogic()
        {

            VoFrameParamManager.Instance.UnRegisterFrameParam();
        }

        public void UpdateFrameLogic()
        {

        }
        private bool gui = false;
        public void SetGuiEnable(bool guiEnable)
        {
            gui = guiEnable;
        }
        public void GUILogic()
        {
            if (!gui)
                return;
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));

            _scrollBar = GUILayout.BeginScrollView(_scrollBar, false, false);

            foreach (FrameConfigData frame in ConfigDataManager.Instance.frameConfigDataDic.Values)
            {
                if (GUILayout.Button(frame.name, GUILayout.Width(200), GUILayout.Height(30)))
                {
                    SelectFrame(frame);
                    gui = false;
                    
                }

            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
