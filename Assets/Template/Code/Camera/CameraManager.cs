using Framework;

namespace Cam
{
    public class CameraManager : BaseManager
    {

        static CameraManager instance = new CameraManager();//1

        public static CameraManager Instance { get { return instance; } }

        private CameraManager() : base(AreaCode.Camera)//2
        {//10

        }

        public void Init() { }

        //Camera main, uiNormal, uiPerspective;

        //public Camera Main { get { return main; } }
        //public Camera UiNormal { get { return uiNormal; } }
        //public Camera UiPerspective { get { return uiPerspective; } }

    }
}