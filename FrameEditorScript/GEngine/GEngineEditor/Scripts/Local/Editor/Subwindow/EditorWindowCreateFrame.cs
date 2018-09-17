using UnityEditor;
using FrameLogicData;
namespace GEngine.Editor
{
    public class EditorWindowCreateFrame : EditorWindow
    {
        EditorViewCreateFrame _viewCreateFrame;
        VoFrameDetailData _frameDetailData;
        VoFrameConfigData _frameConfigData;
        void OnEnable()
        {
            _viewCreateFrame = EditorViewCreateFrame.Instance;
            _viewCreateFrame.Enter();
            _frameConfigData = VoConfigDataManager.Instance.CreateFrameConfigData();
            _viewCreateFrame.GenerateGUIContent();
        }
        void OnDestroy()
        {
            _viewCreateFrame.Exit();
        }
        void OnGUI()
        {
            _viewCreateFrame.DrawUI(_frameConfigData);
        }
    }
}
