namespace GEngine.Editor
{
    interface IEditorView
    {
        void Enter();
        void GenerateGUIContent();
        void DrawUI(object param = null);
        void Exit();
    }
}
