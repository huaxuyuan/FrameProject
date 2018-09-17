using Runtime.FrameLogic;
namespace Runtime.Mono
{
    public class BootMono : SingletonMono<BootMono>
    {
        void Awake()
        {
            FrameLogicManager.Instance.InitializeFrame();
        }
        void OnGUI()
        {
            FrameLogicManager.Instance.GUILogic();
        }
        void Update()
        {

        }
    }
}
