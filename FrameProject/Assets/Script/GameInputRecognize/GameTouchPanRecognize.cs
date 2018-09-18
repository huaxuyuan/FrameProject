using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ClassPool(GameInputType.INPUT_TYPE_PAN)]
public class GameTouchPanRecognize : GameTouchRecognizeBase {

    public GameTouchPanRecognize()
    {
        touchType = GameInputType.INPUT_TYPE_PAN;
    }
}
