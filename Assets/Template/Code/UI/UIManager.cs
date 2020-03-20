using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace UI
{
    public class UIManager : BaseManager
    {
        static UIManager instance = new UIManager();

        public static UIManager Instance { get { return instance; } }

        private UIManager() : base(AreaCode.UI)
        {
            panelDict = new Dictionary<UIPanelType, BaseUI>();
            panelList = new List<BaseUI>();
        }

        Dictionary<UIPanelType, BaseUI> panelDict;
        Transform uiNormal, ui3D;
        List<BaseUI> panelList;

        public void Init(Transform ui2d, Transform ui3d)
        {
            if (ui2d != null && ui3d != null)
            {
                uiNormal = ui2d;
                ui3D = ui3d;
                PushPanel(UIPanelType.Panel_Empty_Normal);
            }
        }

        public BaseUI PushPanel(UIPanelType panelType, bool isExit = false)
        {
            BaseUI topPanel = null;
            if (panelList.Count > 0 && isExit)
            {
                topPanel = panelList[panelList.Count - 1];
                topPanel.OnExit();
            }
            BaseUI panel = GetPanel(panelType);
            if (!panelList.Contains(panel))
            {
                panelList.Add(panel);
                panel.OnEnter();
            }
            return panel;
        }

        public void PopPanel(UIPanelType panelType, bool isDestory = false)
        {
            if (panelList == null || panelList.Count <= 0)
                return;

            BaseUI topPanel = panelDict[panelType];
            topPanel.OnExit();
            panelList.Remove(topPanel);
            if (isDestory)
            {
                GameObject.Destroy(topPanel.gameObject);
                panelDict.Remove(panelType);
            }
        }

        public void PopPanel(BaseUI panelType)
        {
            if (panelList == null || panelList.Count <= 0)
                return;
            panelType.OnExit();
            panelList.Remove(panelType);
        }

        public BaseUI GetPanel(UIPanelType panelType)
        {
            BaseUI panel = null;
            if (!panelDict.ContainsKey(panelType))
            {
                string name = Enum.GetName(typeof(UIPanelType), panelType);
                GameObject instPanel = GameObject.Instantiate(Resources.Load("Panel/" + name)) as GameObject;
                string str = name.Split('_')[2];
                if (str == "Normal")
                {
                    instPanel.transform.SetParent(uiNormal);
                }
                else if (str == "3D")
                {
                    instPanel.transform.SetParent(ui3D);
                }
                instPanel.transform.localEulerAngles = Vector3.zero;
                instPanel.transform.localScale = Vector3.one;
                RectTransform recTF = (instPanel.transform as RectTransform);
                recTF.anchoredPosition3D = Vector3.zero;
                recTF.offsetMax = Vector2.zero;
                recTF.offsetMin = Vector2.zero;

                panel = instPanel.GetComponent<BaseUI>();
                panelDict.Add(panelType, panel);
            }
            else panel = panelDict[panelType];

            return panel;
        }

    }
}