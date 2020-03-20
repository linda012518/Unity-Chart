using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class MsgCenter// : Singleton<MsgCenter>
    {
        public static Dictionary<AreaCode, BaseManager> dicManager = new Dictionary<AreaCode, BaseManager>();//5

        public static void Dispatch(AreaCode areaCode, int eventCode, params object[] message)
        {
            if (dicManager.ContainsKey(areaCode))
                dicManager[areaCode].Execute(eventCode, message);
            else
                Debug.LogWarning("MsgCenter.Dispatch:  this manager is not init");
        }

    }
}