using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class BaseMono<T> : MonoBehaviour, IExecuteHandler where T : BaseManager//, new()
    {
        protected List<int> list = new List<int>();
        protected T mgr;

        public virtual void Execute(int eventCode, params object[] message)
        {

        }

        protected void Bind(params int[] eventCodes)
        {
            for (int i = 0; i < eventCodes.Length; i++)
            {
                if (!list.Contains(eventCodes[i]))
                    list.Add(eventCodes[i]);
            }
            mgr.Add(list.ToArray(), this);
        }

        private void UnBind()
        {
            if (list != null && list.Count > 0)
            {
                mgr.Remove(list.ToArray(), this);
                list.Clear();
            }
        }

        public virtual void OnDestroy()
        {
            if (list != null)
                UnBind();
        }

        protected void Dispatch(AreaCode areaCode, int eventCode, params object[] message)
        {
            MsgCenter.Dispatch(areaCode, eventCode, message);
        }

        protected void DelayDispatch(float delay, AreaCode areaCode, int eventCode, params object[] message)
        {
            StartCoroutine(DelaDispatch(delay, areaCode, eventCode, message));
        }

        IEnumerator DelaDispatch(float delay, AreaCode areaCode, int eventCode, params object[] message)
        {
            yield return new WaitForSeconds(delay);
            Dispatch(areaCode, eventCode, message);
        }

    }
}