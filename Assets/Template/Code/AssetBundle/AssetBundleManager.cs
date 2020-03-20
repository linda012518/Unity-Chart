using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Assetbundle
{
    /// <summary>单个物体  有可能 多个 存取</summary>
    public class AssetObj
    {
        public Dictionary<string, Object> dicObjs;

        public AssetObj(params Object[] tmpObj)
        {
            dicObjs = new Dictionary<string, Object>();
            for (int i = 0; i < tmpObj.Length; i++)
            {
                dicObjs.Add(tmpObj[i].name, tmpObj[i]);
            }
        }

        /// <summary>强办释放内存中的资源</summary>
        public void ReleaseObj()
        {
            foreach (Object item in dicObjs.Values)
            {
                Resources.UnloadAsset(item);
            }
        }

    }

    /// <summary>
    /// 记录已经被管理的bundle包，并已经或即将在内存中产生镜像
    /// </summary>
    public class AssetBundleManager
    {

        static AssetBundleManager instance = new AssetBundleManager();

        public static AssetBundleManager Instance { get { return instance; } }

        private AssetBundleManager() { }

        // 加载出来的object 都存这
        Dictionary<string, AssetObj> loadObj = new Dictionary<string, AssetObj>();

        /// <summary>
        /// 释放包中所有的Obj并释放没有被引用的Obj
        /// </summary>
        /// <param name="bundleName">AB名</param>
        public void DisposeResObj(string bundleName)
        {
            if (loadObj.ContainsKey(bundleName))
            {
                AssetObj tmpObj = loadObj[bundleName];
                tmpObj.ReleaseObj();
            }
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 释放所有的包中的Obj以及没有被引用的Obj，并清空loadObj字典
        /// </summary>
        public void DisposeAllObj()
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadObj.Keys);
            for (int i = 0; i < loadObj.Count; i++)
            {
                DisposeResObj(keys[i]);
            }
            loadObj.Clear();
        }

        public bool IsLoad(string path)
        {
            return loadObj.ContainsKey(Path.GetFileName(path));
        }

        //加载资源
        public IEnumerator LoadAssetBundle(string path, LoadFinish loadFinish = null)
        {
            WWW w = new WWW(path);
            yield return w;
            if (!string.IsNullOrEmpty(w.error))
            {
                Debug.LogWarning("加载 bundle 失败" + w.error);
                yield break;
            }
            AssetBundle ab = w.assetBundle;
            if (loadObj.ContainsKey(ab.name) == false)
                loadObj.Add(ab.name, new AssetObj(ab.LoadAllAssets()));
            if (loadFinish != null) loadFinish();
            w.Dispose();
        }

        public bool IsLoadObj(string key)
        {
            return loadObj.ContainsKey(key);
        }

        /// <summary>
        /// 获取一个Obj，假如加载过了，则在loadObj中直接获取，否则从bundle中加载并记录在loadObj中
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public Object GetSingleResources(string bundleName, string resName)
        {
            //加载过资源
            if (loadObj.ContainsKey(bundleName)) return loadObj[bundleName].dicObjs[resName];
            return null;
        }

        /// <summary>
        /// 获取一个集类型（含有子物体）的Obj
        /// </summary>
        /// <param name="bundleName"></param>
        public Dictionary<string, Object> GetMutiResources(string bundleName)
        {
            if (loadObj.ContainsKey(bundleName)) return loadObj[bundleName].dicObjs;
            return null;
        }
    }


    public delegate void LoadFinish();

}