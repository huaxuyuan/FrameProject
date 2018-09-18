using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
public class GameInputType
{
    public const int INPUT_TYPE_TAP = 1;
    public const int INPUT_TYPE_SWIP = 2;
    public const int INPUT_TYPE_SCALE = 3;
    public const int INPUT_TYPE_PAN = 4;
}
public class GameRayCastMask
{

}
public class GameInputManager : SingletonMono<GameInputManager>
{
    public GameObject fingerGameObject;
    private FingersScript _fingersLogic;
    private ClassObjPoolReflectionWithKey<GameTouchRecognizeBase> _touchRecognizePool;
    private Dictionary<int, GameTouchRecognizeBase> _recognizeKeyIsHashCode;

    private TapGestureRecognizer _tapGesture;
    private TapGestureRecognizer _doubleTapGesture;
    private TapGestureRecognizer _tripleTapGesture;
    private SwipeGestureRecognizer _swipeGesture;
    private PanGestureRecognizer _panGesture;
    private ScaleGestureRecognizer _scaleGesture;
    private RotateGestureRecognizer _rotateGesture;
    private LongPressGestureRecognizer _longPressGesture;

    private Dictionary<int, List<GameTouchRecognizeBase>> _touchRecognizeParamDic;
    private Camera _rayCastCamera;
    private RaycastHit _hit;
    private void Awake()
    {
        _fingersLogic = fingerGameObject.GetComponent<FingersScript>();

        _touchRecognizePool = new ClassObjPoolReflectionWithKey<GameTouchRecognizeBase>();
        _touchRecognizePool.AddReflectionClass(new GameTouchPressRecognize());
        _touchRecognizePool.AddReflectionClass(new GameTouchScaleRecognize());
        _touchRecognizePool.AddReflectionClass(new GameTouchSwipRecognize());
        _touchRecognizePool.AddReflectionClass(new GameTouchPanRecognize());

        _touchRecognizePool.GetAllReflectClass();
        _touchRecognizeParamDic = new Dictionary<int, List<GameTouchRecognizeBase>>();
        _rayCastCamera = null;
    }
    public void SetRayCastCamera(Camera camera)
    {
        _rayCastCamera = camera;
    }
    public GameTouchPressRecognize AddTouchOnePress()
    {
        if(_tapGesture == null)
        {
            CreateTapGesture();
        }
        GameTouchPressRecognize gametouch = GetTouchRecognize(GameInputType.INPUT_TYPE_TAP) as GameTouchPressRecognize;
        _touchRecognizeParamDic[GameInputType.INPUT_TYPE_TAP].Add(gametouch);
        return gametouch;

    }
    
    public GameTouchScaleRecognize AddTouchScale()
    {
        if(_scaleGesture == null)
        {
            CreateScaleTouch();
        }
        GameTouchScaleRecognize gametouch = GetTouchRecognize(GameInputType.INPUT_TYPE_SCALE) as GameTouchScaleRecognize;
        _touchRecognizeParamDic[GameInputType.INPUT_TYPE_SCALE].Add(gametouch);
        return gametouch;
    }
    public GameTouchSwipRecognize AddTouchSwip()
    {
        if(_swipeGesture == null)
        {
            CreateSwipeGesture();
        }
        GameTouchSwipRecognize gametouch = GetTouchRecognize(GameInputType.INPUT_TYPE_SWIP) as GameTouchSwipRecognize;
        _touchRecognizeParamDic[GameInputType.INPUT_TYPE_SWIP].Add(gametouch);
        return gametouch;
    }
    public GameTouchPanRecognize AddPanRecognize()
    {
        if(_panGesture == null)
        {
            CreatePanGesture();
        }
        GameTouchPanRecognize gametouch = GetTouchRecognize(GameInputType.INPUT_TYPE_PAN) as GameTouchPanRecognize;
        _touchRecognizeParamDic[GameInputType.INPUT_TYPE_PAN].Add(gametouch);
        return gametouch;
    }
    public void RemoveTouchRecognize(GameTouchRecognizeBase pressTouch)
    {
        if (pressTouch == null)
            return;
        if (_touchRecognizeParamDic[pressTouch.touchType].Contains(pressTouch))
        {
            _touchRecognizePool.RecycleCloneObj(pressTouch.touchType, pressTouch);
            _touchRecognizeParamDic[pressTouch.touchType].Remove(pressTouch);
        }
    }

    public void ClearAllInputVariable()
    {
        _tapGesture = null;
        _doubleTapGesture = null;
        _longPressGesture = null;
        _panGesture = null;
        _swipeGesture = null;
        _scaleGesture = null;
        _rotateGesture = null;
        _tripleTapGesture = null;

        foreach(List<GameTouchRecognizeBase> recognizeList in _touchRecognizeParamDic.Values)
        {
            foreach(GameTouchRecognizeBase touch in recognizeList)
            {
                _touchRecognizePool.RecycleCloneObj(touch.touchType, touch);
            }
            recognizeList.Clear();
        }
        _touchRecognizeParamDic.Clear();
    }
    private GameTouchRecognizeBase GetTouchRecognize(int touchType)
    {
        return _touchRecognizePool.GetCloneObj(touchType);
    }

    private void CreateTapGesture()
    {
        _tapGesture = new TapGestureRecognizer();
        _tapGesture.StateUpdated += TapGestureCallback;
        _fingersLogic.AddGesture(_tapGesture);
        _touchRecognizeParamDic.Add(GameInputType.INPUT_TYPE_TAP, new List<GameTouchRecognizeBase>());
    }
    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Tapped at {0}, {1}"+ gesture.FocusX+ gesture.FocusY);
            if(_rayCastCamera == null)
            {
                return;
            }
           
            if(!_touchRecognizeParamDic.ContainsKey(GameInputType.INPUT_TYPE_TAP) || _touchRecognizeParamDic[GameInputType.INPUT_TYPE_TAP].Count == 0)
            {
                return;
            }
            List<GameTouchRecognizeBase> touchList = _touchRecognizeParamDic[GameInputType.INPUT_TYPE_TAP];
            Vector3 screenPos = new Vector3(gesture.FocusX, gesture.FocusY, 0);
            Ray ray = _rayCastCamera.ScreenPointToRay(screenPos);
            foreach (GameTouchRecognizeBase touch in touchList)
            {
                if (Physics.Raycast(ray, out _hit, 1000, touch.rayCastMask))
                {
                    if(touch.JudgeRayCastLogic(_hit))
                    {
                        return;
                    }
                }
            }

        }
    }
    private void CreateScaleTouch()
    {
        _scaleGesture = new ScaleGestureRecognizer();
        _scaleGesture.StateUpdated += ScaleGestureCallback;
        _fingersLogic.AddGesture(_scaleGesture);
        _touchRecognizeParamDic.Add(GameInputType.INPUT_TYPE_SCALE, new List<GameTouchRecognizeBase>());
    }
    private void ScaleGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
           // DebugText("Scaled: {0}, Focus: {1}, {2}", scaleGesture.ScaleMultiplier, scaleGesture.FocusX, scaleGesture.FocusY);
           
        }
    }
    private void CreateSwipeGesture()
    {
        _swipeGesture = new SwipeGestureRecognizer();
        _swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
        _swipeGesture.StateUpdated += SwipeGestureCallback;
        _swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        _fingersLogic.AddGesture(_swipeGesture);
        _touchRecognizeParamDic.Add(GameInputType.INPUT_TYPE_SWIP, new List<GameTouchRecognizeBase>());
    }
    private void SwipeGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //HandleSwipe(gesture.FocusX, gesture.FocusY);
            //DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
        }
    }
    private void CreatePanGesture()
    {
        _panGesture = new PanGestureRecognizer();
        _panGesture.MinimumNumberOfTouchesToTrack = 2;
        _panGesture.StateUpdated += PanGestureCallback;
        _fingersLogic.AddGesture(_panGesture);
        _touchRecognizeParamDic.Add(GameInputType.INPUT_TYPE_PAN, new List<GameTouchRecognizeBase>());
    }
    private void PanGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (!_touchRecognizeParamDic.ContainsKey(GameInputType.INPUT_TYPE_PAN) || _touchRecognizeParamDic[GameInputType.INPUT_TYPE_PAN].Count == 0)
        {
            return;
        }
        if (gesture.State != GestureRecognizerState.Executing)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                List<GameTouchRecognizeBase> touchList = _touchRecognizeParamDic[GameInputType.INPUT_TYPE_PAN];
                foreach (GameTouchRecognizeBase touch in touchList)
                {
                    touch.OnBeginTouch(gesture);
                }
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                List<GameTouchRecognizeBase> touchList = _touchRecognizeParamDic[GameInputType.INPUT_TYPE_PAN];
                foreach (GameTouchRecognizeBase touch in touchList)
                {
                    touch.OnEndTouch(gesture);
                }
            }

        }
        else
        {
            List<GameTouchRecognizeBase> touchList = _touchRecognizeParamDic[GameInputType.INPUT_TYPE_PAN];
            foreach (GameTouchRecognizeBase touch in touchList)
            {
                touch.OnExecuteTouch(gesture);
            }
        }
    }
}
