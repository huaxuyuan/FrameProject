

namespace FrameEditorLogic
{
    public class EditorFrameConfigLogic : SingletonNotMono<EditorFrameConfigLogic>
    {
        public void InitializeFrameLogic()
        {
            GEngine.Editor.EditorViewFrame.Instance.InitializeFrameLogic();
        }
    }
}
