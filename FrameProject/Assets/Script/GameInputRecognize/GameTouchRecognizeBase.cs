using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
public class GameTouchRecognizeBase : IClassObjWithoutKey
{
    public int touchType;
    public int rayCastMask;
    
    public virtual bool CheckRecognizeSuccess(GestureRecognizer gestureRecognizer)
    {
        return false;
    }
    public virtual bool JudgeRayCastLogic(RaycastHit castHitData)
    {
        return false;
    }
    public virtual void OnBeginTouch(GestureRecognizer gestureRecognizer)
    {

    }
    public virtual void OnExecuteTouch(GestureRecognizer gestureRecognizer)
    {

    }
    public virtual void OnEndTouch(GestureRecognizer gestureRecognizer)
    {

    }
    public virtual void Clear()
    {
        
    }

    public virtual void Recycle()
    {
        
    }
}
