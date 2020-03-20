
public static class JsonUtil
{
    
    public static string ToJson<T>(T obj)
    {
        if (obj == null) return "null";

        if (typeof(T).GetInterface("IList") != null)
        {
            Pack<T> pack = new Pack<T>();
            pack.data = obj;
            string json = UnityEngine.JsonUtility.ToJson(pack);
            return json.Substring(8, json.Length - 9);
        }

        return UnityEngine.JsonUtility.ToJson(obj);
    }

    public static T ToObject<T>(string json)
    {
        if (json == "null" && typeof(T).IsClass) return default(T);

        if (typeof(T).GetInterface("IList") != null)
        {
            json = "{\"data\":{data}}".Replace("{data}", json);
            Pack<T> Pack = UnityEngine.JsonUtility.FromJson<Pack<T>>(json);
            return Pack.data;
        }

        return UnityEngine.JsonUtility.FromJson<T>(json);
    }

    private class Pack<T>
    {
        public T data;
    }

}