using UnityEngine;

namespace UI
{
    public class Panel_Empty_Normal : BaseUI
    {
        public override void Awake()
        {
            base.Awake();
            Bind(UIEvent.Push);
        }

        public override void Execute(int eventCode, params object[] message)
        {
            switch (eventCode)
            {
                case UIEvent.Push:
                    if (message != null)
                        Push(message[0]);
                    else
                        Debug.LogWarning("Panel_Empty_Normal.Execute.Push:  message is null");
                    break;
            }
        }

        void Push(object obj)
        {
            UIPanelType pp;
            if (System.Enum.TryParse<UIPanelType>(obj.ToString(), out pp))
            {
                mgr.PushPanel(pp);
            }
            else
            {
                Debug.LogWarning("Panel_Empty_Normal.Execute.Push:  message[0] is not UIPanelType class");
            }
        }

    }
}