
using UnityEngine;

namespace Freamwork
{
    public enum PlatformEnum
    {
        IOS,
        Android,
        PC,
        None,
    }

    public partial class Global
    {

        public static PlatformEnum Platform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:
                        return PlatformEnum.PC;
                    case RuntimePlatform.Android:
                        return PlatformEnum.Android;
                    case RuntimePlatform.IPhonePlayer:
                        return PlatformEnum.IOS;
                    default:
                        return PlatformEnum.None;
                }
            }
        }


    }
}
