using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GEngine.Editor
{
    public class CustomEditorGUILayout
    {
        public static int ToogleGroup(int selected, bool[] toogleValueArray,GUIContent[] label,GUIStyle labelStyle,params GUILayoutOption[] options)
        {
            for (int i = 0; i < toogleValueArray.Length; i++)
            {
                // public static bool ToggleLeft(GUIContent label, bool value, GUIStyle labelStyle, params GUILayoutOption[] options);
                toogleValueArray[i] = EditorGUILayout.ToggleLeft(label[i], toogleValueArray[i], labelStyle, options);
                if (i == selected)
                {
                    continue;
                }
                if (toogleValueArray[i])
                {
                    toogleValueArray[selected] = false;
                    selected = i;
                }
            }
            return selected;
        }
    }
}
