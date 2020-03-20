using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assetbundle
{
    public class AssetbundlesMenu
    {

        [MenuItem("Tools/AssetBundles/Mark Asset", false, 100)]
        public static void MarkAssetBundle()
        {
            //<<AssetDatabase:在Editor状态下进行文件的操作>>
            AssetDatabase.StartAssetEditing();
            //移除未使用的assetBundle的名称
            AssetDatabase.RemoveUnusedAssetBundleNames();

            //AssetBundles资源路径
            string path = Application.dataPath + "/Bundle/";
            DirectoryInfo dir = new DirectoryInfo(path);

            FileSystemInfo[] filesInfo = dir.GetFileSystemInfos();
            for (int i = 0; i < filesInfo.Length; i++)
            {
                //<<EditorUtility : Editor下显示进度条>>
                EditorUtility.DisplayProgressBar("Mark", "Mark", (float)i / filesInfo.Length);

                FileSystemInfo tmpFile = filesInfo[i];
                //如果文件是文件夹：
                if (tmpFile is DirectoryInfo)
                {
                    string tmpPath = Path.Combine(path, tmpFile.Name);
                    //一般是一个场景内的文件放在一个文件夹内:
                    //例如【场景文件夹ScenceOne】：***Art\Scences\ScenceOne
                    BuildScript.MarkAssetBundle(tmpPath);
                }
            }

            EditorUtility.DisplayProgressBar("Mark", "Mark", 1.0f);
            EditorUtility.ClearProgressBar();
            AssetDatabase.StopAssetEditing();
        }


        [MenuItem("Tools/AssetBundles/Build AssetBundles", false, 100)]
        public static void BuildAssetBundle()
        {
            BuildScript.BuildAssetBundles();
        }

    }
}