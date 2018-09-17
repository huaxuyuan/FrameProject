using System;

public delegate void CallbackDelegate();
public delegate void CallbackDelegate<T1>(T1 t1);
public delegate void CallbackDelegate<T1, T2>(T1 t1, T2 t2);
public delegate void CallbackDelegate<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
public delegate bool BoolCallBackDelegate();
public delegate bool BoolCallBackDelegate<T1>(T1 t1);
public delegate bool BoolCallBackDelegate<T1, T2>(T1 t1, T2 t2);
public delegate bool BoolCallBackDelegate<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

public delegate T CallBackWithReturn<T>();
public delegate Object CallBackWithReturnObject<T>(T t1);
