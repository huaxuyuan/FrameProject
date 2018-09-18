using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ClassPool(GameInputType.INPUT_TYPE_TAP)]
public class GameTouchPressRecognize : GameTouchRecognizeBase
{
    public CallbackDelegate<GameObject> pressCallBack;
    public GameTouchPressRecognize()
    {
        touchType = GameInputType.INPUT_TYPE_TAP;
    }
    public override bool CheckRecognizeSuccess(GestureRecognizer gestureRecognizer)
    {
        return base.CheckRecognizeSuccess(gestureRecognizer);
    }
    public override void Clear()
    {
    }

    public override void Recycle()
    {
        
    }

}
