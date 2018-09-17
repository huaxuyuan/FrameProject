using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

public interface IClassObj 
{
	void Initialize(int keyValue);
	void Recycle ();
	void Clear();
}

public interface IClassObjWithoutKey 
{
	void Recycle ();
	void Clear ();
}

public  class ClassObjPoolWithoutKey<ClassObjType>
	where ClassObjType : class,IClassObjWithoutKey,new()
{
	private Queue<ClassObjType> _classObjQue;
	public ClassObjPoolWithoutKey()
	{
		_classObjQue = new Queue<ClassObjType> ();
	}
	public  ClassObjType GetObj()
	{
		ClassObjType classObj;
		if (_classObjQue.Count >0)
		{
			classObj = _classObjQue.Dequeue();
			//Debug.Log ("Dequeue classObj"+ classObj);
			return classObj;
		}
		classObj = new ClassObjType ();
		//Debug.Log ("new classOb"+classObj);
		return classObj;
	}
	public void RecycleCloneObj(ClassObjType objValue)
	{
		if (objValue == null)
			return;
		objValue.Recycle ();
        if (!_classObjQue.Contains(objValue))
        {
            _classObjQue.Enqueue(objValue);
        }
        else
        {
            //Debug.LogError("HaveThis");
        }
	}
	public void Clear()
	{
        if (_classObjQue.Count > 0)
        {
            ClassObjType classObj;
            do
            {
                classObj = _classObjQue.Dequeue();
                classObj.Clear();
                classObj = null;
            }
            while (_classObjQue.Count > 0);
            _classObjQue.Clear();
        }
        //int _classQueCount = _classObjQue.Count;
        //ClassObjType classObj;

        //for (int index = 0;index < _classQueCount;++index)
        //{
        //    classObj = _classObjQue.Dequeue();
        //    classObj.Recycle();
        //    _classObjQue.Enqueue(classObj);
        //}
        //if (_classObjQue.Count > 0)
        //{
        //    ClassObjType classObj;
        //    do
        //    {
        //        classObj = _classObjQue.Dequeue();
        //        classObj.Recycle();
        //        _classObjQue.Enqueue(classObj);
        //    }
        //    while (_classObjQue.Count > 0);
        //    _classObjQue.Clear();
        //}
        //_classObjQue = null;
    }
	
}

public  class ClassObjPoolWithKey<ClassObjType>
	where ClassObjType : class,IClassObj,new()
{
	private Dictionary<int,ClassObjType> classObjDic;
	public ClassObjPoolWithKey()
	{
		classObjDic = new Dictionary<int, ClassObjType> ();
	}
	public  ClassObjType GetObj(int keyValueInt)
	{
		ClassObjType classObj;
		if (classObjDic.ContainsKey (keyValueInt))
		{
			classObj = classObjDic [keyValueInt];
			classObj.Initialize(keyValueInt);
			return classObj;
		}
        
        classObj = new ClassObjType ();
		classObj.Initialize (keyValueInt);
		classObjDic.Add (keyValueInt, classObj);
		return classObj;
	}
	public void Clear()
	{
		if(classObjDic.Count > 0)
		{
			foreach(ClassObjType classObj in classObjDic.Values)
			{
				classObj.Clear();
			}
			classObjDic.Clear();
		}
		classObjDic = null;
	}

}

public  class CloneClassObjPoolWithKey<ClassObjType>
	where ClassObjType : class,IClassObj,new()
{
	private Dictionary<int,Queue<ClassObjType>> _cloneClassObjDic;
	public CloneClassObjPoolWithKey()
	{
		_cloneClassObjDic = new Dictionary<int, Queue<ClassObjType>> ();
	}
	public ClassObjType GetCloneObj(int keyValueInt)
	{
		Queue<ClassObjType> queueValue;
		ClassObjType classObj;
		if(!_cloneClassObjDic.ContainsKey(keyValueInt))
		{
//			DebugTools.LogError("_cloneClassObjDic !contains_" +keyValueInt);
			queueValue = new Queue<ClassObjType>();
			_cloneClassObjDic.Add (keyValueInt,queueValue);
		}
		queueValue = _cloneClassObjDic [keyValueInt];
		
		if (queueValue.Count > 0)
		{
			classObj = queueValue.Dequeue ();
			classObj.Initialize (keyValueInt);
			return classObj;
		}

		classObj = new ClassObjType ();
		classObj.Initialize (keyValueInt);
		return classObj;
	}
	public void RecycleCloneObj(int keyValueInt,ClassObjType objValue)
	{
		objValue.Recycle ();
		_cloneClassObjDic [keyValueInt].Enqueue (objValue);
	}

	public void Clear()
	{
		if(_cloneClassObjDic.Count > 0)
		{
			ClassObjType classObj;
			foreach(Queue<ClassObjType> _classObjQue in _cloneClassObjDic.Values)
			{
				if(_classObjQue.Count > 0)
				{
					do
					{
						classObj = _classObjQue.Dequeue();
						classObj.Clear();
						classObj = null;
					}
					while(_classObjQue.Count > 0);
					_classObjQue.Clear();
				}
			}
			_cloneClassObjDic.Clear();
		}
		_cloneClassObjDic = null;
	}

}

/*
 * 
 * public class TestClassObj : IClassObj
{
	bool hasUsed;

	public bool used
	{
		get
		{
			return hasUsed;
		}
	}
	public void Initialize()
	{
		hasUsed = true;
	}
	public void Recycle ()
	{
		hasUsed = false;
	}
}
 * 		
		ClassObjPool<TestClassObj> testClassObj = new ClassObjPool<TestClassObj> ();
		TestClassObj classObj =  testClassObj.GetObj (1);
		classObj.Recycle ();
		Debug.Log (classObj.used);

		 classObj =  testClassObj.GetObj (1);
		Debug.Log (classObj.used);
		

*/

public class ClassObjPoolReflectionWithKey<ClassObjType>
where ClassObjType : class, IClassObjWithoutKey, new()
{
    private Dictionary<int, Type> _cloneContructorInfoDic;
    private List<ClassObjType> _classTypeList;
    private Dictionary<int, Queue<ClassObjType>> _classPool;
    public ClassObjPoolReflectionWithKey()
    {
        _cloneContructorInfoDic = new Dictionary<int, Type>();
        _classPool = new Dictionary<int, Queue<ClassObjType>>();
        _classTypeList = new List<ClassObjType>();
    }
    public void AddReflectionClass(ClassObjType classObject)
    {
        if (classObject == null)
            return;
        if (_classTypeList.Contains(classObject))
            return;
        _classTypeList.Add(classObject);
    }
    public Type GetReflectionType(int typeName)
    {
        if (_cloneContructorInfoDic.ContainsKey(typeName))
            return _cloneContructorInfoDic[typeName];
        return null;
    }
    public void GetAllReflectClass()
    {
        foreach(ClassObjType obj in _classTypeList)
        {
            object[] attrs = obj.GetType().GetCustomAttributes(typeof(ClassPoolAttribute), false);
            
            for(int i = 0;i < attrs.Length;++i)
            {
                ClassPoolAttribute classPoolArribute = attrs[i] as ClassPoolAttribute;
                _cloneContructorInfoDic[classPoolArribute.ClassName] = obj.GetType();
                Debug.Log(classPoolArribute.ClassName + ""+ obj.GetType());
            }
        }
    }
    public ClassObjType GetCloneObj(int typeName)
    {
        if(!_cloneContructorInfoDic.ContainsKey(typeName))
        {
            Debug.LogError("reflect pool not contains type "+typeName);
            return null;
        }
        ClassObjType classObjResult;
        if (_classPool.ContainsKey(typeName) && _classPool[typeName].Count > 0)
        {
            classObjResult = _classPool[typeName].Dequeue();
        }
        else
            classObjResult = Activator.CreateInstance(_cloneContructorInfoDic[typeName]) as ClassObjType;
        return classObjResult;
    }
    public void RecycleCloneObj(int typeName,ClassObjType obj)
    {
        if (obj == null)
            return;
        if (!_cloneContructorInfoDic.ContainsKey(typeName))
        {
            return;
        }
        if(!_classPool.ContainsKey(typeName))
        {
            _classPool.Add(typeName, new Queue<ClassObjType>());
        }
        if(!_classPool[typeName].Contains(obj))
        {
            obj.Recycle();
            _classPool[typeName].Enqueue(obj);
        }
           
    }
	public void Clear()
	{
		if (_classPool.Count > 0)
		{
			ClassObjType classObj;
			foreach (Queue<ClassObjType> _classObjQue in _classPool.Values)
			{
				if (_classObjQue.Count > 0)
				{
					do
					{
						classObj = _classObjQue.Dequeue();
						classObj.Clear();
						classObj = null;
					}
					while (_classObjQue.Count > 0);
					_classObjQue.Clear();
				}
			}
			_classPool.Clear();
		}
		_classPool = null;
        _classTypeList.Clear();
        _cloneContructorInfoDic.Clear();

    }


}