using System;
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ClassPoolAttribute : Attribute
{
    readonly int className;
    public ClassPoolAttribute(int prefabName)
    {
        this.className = prefabName;
    }

    public int ClassName
    {
        get { return className; }
    }
}
