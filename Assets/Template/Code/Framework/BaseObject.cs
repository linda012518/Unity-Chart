using System.Collections.Generic;

namespace Framework
{
    public class BaseObject<T> : IExecuteHandler where T : BaseManager
    {

        protected List<int> list = new List<int>();
        protected T mgr;

        public virtual void Execute(int eventCode, params object[] message)
        {

        }

        protected void Bind(params int[] eventCodes)
        {
            list.AddRange(eventCodes);
            mgr.Add(list.ToArray(), this);
        }

        protected void UnBind()
        {
            mgr.Remove(list.ToArray(), this);
            list.Clear();
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
    }
}