using UnityEngine;
using System.Collections;

//base state
public abstract class State<T>
{
	//T target
	public T Target ;
	//Enter
	public abstract void Enter (T type);
	//Execute
	public abstract void Execute (T type,float timeSpan);
	//Exit
	public abstract void Exit (T type);

}


