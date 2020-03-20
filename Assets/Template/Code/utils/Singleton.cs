using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T instance = null;
		public static T Instance {
			get 
            {
				if (instance == null)
				{
					instance = FindObjectOfType(typeof(T)) as T;
					if (instance == null)
					{
						instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
						DontDestroyOnLoad(instance);
					}
				}
				return instance;
			}
		}
		
		void OnApplicationQuit () { if (instance != null) instance = null; }
	}
}