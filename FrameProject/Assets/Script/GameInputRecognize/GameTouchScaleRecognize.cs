using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ClassPool(GameInputType.INPUT_TYPE_SCALE)]
public class GameTouchScaleRecognize : GameTouchRecognizeBase
{
    public GameTouchScaleRecognize()
    {
        touchType = GameInputType.INPUT_TYPE_SCALE;
    }
}
