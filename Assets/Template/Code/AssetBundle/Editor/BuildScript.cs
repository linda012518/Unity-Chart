using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assetbundle
{
    public class BuildScript
    {
        /// <summary>
        /// Mark Asset 生成信息文档以及信息字典
        /// </summary>
        public static void MarkAssetBundle(string fullPath)
        {
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            if (dir != null)
            {
                ListFiles(dir);
            }
            else
            {
                Debug.Log("this path  is not exit");
            }
            Debug.Log("Mark Asset has been Finished");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Mark Asset ==> ==>递归遍历文件
        /// </summary>
        public static void ListFiles(FileSystemInfo info)
        {
            //【文件】不存在则返回
            if (!info.Exists)
            {
                Debug.Log("FileSystemInfo is not exit!");
                return;
            }
            //文件转换为【文件夹】
            DirectoryInfo dir = info as DirectoryInfo;
            if (dir == null)
            {
                Debug.Log("DirectoryInfo is not exit!");
                return;
            }
            //【文件夹下的子文件】
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Mark", files[i].FullName, (float)i / files.Length);
                FileInfo file = files[i] as FileInfo;
                //是文件
                if (file != null)
                {
                    ChangerMark(file, dir.Name);
                }
                //对于子目录，进行递归调用
                else
                {
                    ListFiles(files[i]);
                }
            }
        }

        /// <summary>
        /// Mark Asset ==> ==> ==>剔除过渡文件".meta"".ds_store""."
        /// </summary>
        public static void ChangerMark(FileInfo tmpFile, string folderName)
        {
            if (tmpFile.Extension.ToLower() == ".meta" || tmpFile.Extension.ToLower() == ".ds_store")
            {
                return;
            }

            if (tmpFile.Name.StartsWith("."))
                return;

            //更改标记 填入字典
            ChangerAssetMark(tmpFile, folderName);
        }

        /// <summary>
        /// Mark Asset ==> ==> ==> ==>更改标记 填入字典
        /// </summary>
        public static void ChangerAssetMark(FileInfo tmpFile, string folderName)
        {
            string fullPath = tmpFile.FullName;
            //例如：D:\Documents\Unity Projects\Event Frame\Assets\Bundle\sprites.png
            //例如:Assets\Bundle\sprites.png
            string assetPath = fullPath.Substring(fullPath.IndexOf("Assets"));
            //<<把文件导入内存>>
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            //<<更改标记:要标记的路径>>:AssetBundleName字符串
            importer.assetBundleName = folderName;
            importer.assetBundleVariant = "vinfai";
        }


        public static void BuildAssetBundles()
        {
            string outputPath = Application.streamingAssetsPath + "/AssetBundles/" + Path();

            if (!Directory.Exists(outputPath))
            {
                Debug.Log("CreateDirectory:" + outputPath);
                Directory.CreateDirectory(outputPath);
            }
            Debug.Log("outputPath:" + outputPath);
            //BuildAssetBundles：根据assetBundleName和assetBundleVariant在输出路径创建AssetBundles文件
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }

        static string Path()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "PC/";
                case RuntimePlatform.Android:
                    return "Android/";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS/";
                default:
                    return "None/";
            }
        }
    }
}