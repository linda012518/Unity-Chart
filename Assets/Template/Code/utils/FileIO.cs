using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

namespace Utils
{
    /// <summary>
    /// 文件操作类
    /// add by zr
    /// </summary>
    public class FileIO
    {
        /// <summary>
        /// 读取文件，内容以字符串返回
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>内容</returns>
        public static string Read(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string s = sr.ReadToEnd();
                sr.Close();
                return s;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>存在与否</returns>
        public static bool Exists(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("文件:" + path + "不存在！");
                return false;
            }
            return true;
        }

        public static void Save(string path, string content)
        {
            try
            {
                using (FileStream fw = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(content);
                    fw.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 读取所有数据
        /// </summary>
        public static Dictionary<string, Dictionary<string, List<string>>> LoadConfig(string configFile)
        {
            Dictionary<string, Dictionary<string, List<string>>> cache = new Dictionary<string, Dictionary<string, List<string>>>();
            Dictionary<string, List<string>> dic = null;
            List<string> list = null;

            var configPath = Path.Combine(Application.streamingAssetsPath, configFile);
            //WWW www = new WWW(configPath);
            //while (true)
            //{
            //    if (!string.IsNullOrEmpty(www.error))
            //        throw new Exception(www.error);
            //    if (www.isDone)
            //    {
            string mainKey = null;//主键
            string subKey = null;//子键
            string subValue = null;//值

            //StringReader sReader = new StringReader(www.text);
            StreamReader sr = new StreamReader(configPath, Encoding.Default);
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();//去除空白行
                if (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("["))
                    {

                        var configValue = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        subKey = configValue[0].Trim();
                        subValue = configValue[1].Trim();

                        if (!dic.ContainsKey(subKey))
                        {
                            list = new List<string>();
                            dic.Add(subKey, list);
                        }
                        list.Add(subValue);
                    }
                    else
                    {
                        int index = line.IndexOf("]");
                        mainKey = line.Substring(1, index - 1);
                        dic = new Dictionary<string, List<string>>();
                        if (!cache.ContainsKey(mainKey))
                            cache.Add(mainKey, dic);
                    }
                }
            }
            return cache;
        }
        //    }
        //}


        public static Sprite LoadTexture(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //创建Texture
            int width = 300;
            int height = 372;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);

            //创建Sprite
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

    }
}