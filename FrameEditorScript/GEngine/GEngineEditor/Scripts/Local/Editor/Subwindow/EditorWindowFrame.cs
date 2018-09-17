using UnityEditor;
using UnityEngine;
namespace GEngine.Editor
{
    class EditorWindowFrame : EditorWindow
    {
        EditorViewFrame _frameView;
        void OnEnable()
        {
            _frameView = EditorViewFrame.Instance;
            _frameView.Enter();
        }
        void OnDestroy()
        {
            _frameView.Exit();
        }
        void OnGUI()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            _frameView.DrawUI(null);
        }
    }
}
