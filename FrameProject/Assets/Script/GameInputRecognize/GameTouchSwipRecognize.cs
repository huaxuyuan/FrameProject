using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ClassPool(GameInputType.INPUT_TYPE_SWIP)]
public class GameTouchSwipRecognize : GameTouchRecognizeBase
{
    public GameTouchSwipRecognize()
    {
        touchType = GameInputType.INPUT_TYPE_SWIP;
    }
}
