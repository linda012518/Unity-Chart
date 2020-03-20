using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Model;

namespace Utils
{
    public class CityPosition// : MonoBehaviour
    {
        public class Result
        {
            public int status;

            public ResultBody result;
        }

        public class ResultBody
        {
            public Location location;
            public int precise;
            public int confidence;
            public int comprehension;
            public string level;
        }

        public class Location
        {
            public float lng;
            public float lat;
        }
        #region//const
        private static double[] LLBAND = { 75d, 60d, 45d, 30d, 15d, 0d };
        private static Dictionary<int, double[]> LL2MC = new Dictionary<int, double[]>() {
            {0, new double[]{ -0.0015702102444, 111320.7020616939, 1704480524535203d, -10338987376042340d, 26112667856603880d, -35149669176653700d, 26595700718403920d, -10725012454188240d, 1800819912950474d, 82.5 }},
            {1, new double[]{ 0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167, -5124939663.577472, 913311935.9512032, 67.5 }},
            {2, new double[]{ 0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5 }},
            {3, new double[]{ 0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5 }},
            {4, new double[]{ -0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5 }},
            {5, new double[]{ -0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45 }}
        };
        #endregion

        float east_longitude = 135.099291f;//纬度
        float east_latitude = 48.447042f;//

        float south_longitude = 109.651231f;//纬度
        float south_latitude = 18.201131f;//经度

        float west_longitude = 73.51894f;
        float west_latitude = 39.394723f;

        float north_longitude = 123.26523f;
        float north_latitude = 53.584004f;

        string _province;

        float xOffset;//纬度
        float yOffset;//经度
        float westX, southY;

        public Image map;
        public RectTransform cityImg;
        float width;
        float height;

        //void Start()
        //{
        //    Init("中国");
        //    SetCityPos(cityName);
        //}

        public void Init(string province)
        {
            _province = province;
            Coordinate coord = JsonUtility.FromJson<Coordinate>(FileIO.Read(Application.streamingAssetsPath + "/Coordinate/" + _province + ".json"));

            east_latitude = coord.east_latitude;
            east_longitude = coord.east_longitude;
            south_latitude = coord.south_latitude;
            south_longitude = coord.south_longitude;
            west_latitude = coord.west_latitude;
            west_longitude = coord.west_longitude;
            north_latitude = coord.north_latitude;
            north_longitude = coord.north_longitude;

            Dictionary<string, float> eastDic = convertLL2MC(east_longitude, east_latitude);
            Dictionary<string, float> southDic = convertLL2MC(south_longitude, south_latitude);
            Dictionary<string, float> westDic = convertLL2MC(west_longitude, west_latitude);
            Dictionary<string, float> northDic = convertLL2MC(north_longitude, north_latitude);

            xOffset = eastDic["x"] - westDic["x"];
            yOffset = northDic["y"] - southDic["y"];

            westX = westDic["x"];
            southY = southDic["y"];
            width = map.rectTransform.rect.width;
            height = map.rectTransform.rect.height;

            InitCityImagePos();
        }

        void InitCityImagePos()
        {
            Vector2 pos = cityImg.anchoredPosition;
            pos.x = -width / 2;
            pos.y = -height / 2;
            cityImg.anchoredPosition = pos;
        }

        public void SetMapAndCity(Image map, RectTransform cityPoint)
        {
            this.map = map;
            cityImg = cityPoint;
        }

        public void SetCityPos(string city)
        {
            string json = CreateGetHttpResponse(_province + city, "QufTPKOS0yl1mDNB8yxHvnymOKsRAeZ7");
            Result result = JsonConvert.DeserializeObject<Result>(json);
            Location location = result.result.location;

            Dictionary<string, float> dic = convertLL2MC(location.lng, location.lat);

            float lerpX = (dic["x"] - westX) / xOffset;
            float lerpY = (dic["y"] - southY) / yOffset;

            Vector2 pos = cityImg.anchoredPosition;

            pos.x += width * lerpX;
            pos.y += height * lerpY;

            cityImg.anchoredPosition = pos;
        }

        //经纬度坐标转墨卡托坐标
        Dictionary<string, float> convertLL2MC(float lng, float lat)
        {
            double[] cE = null;
            for (int i = 0; i < LLBAND.Length; i++)
            {
                if (lat >= LLBAND[i]) { cE = LL2MC[i]; break; }
            }
            if (cE != null)
            {
                for (int i = LLBAND.Length - 1; i >= 0; i--)
                {
                    if (lat <= -LLBAND[i])
                    {
                        cE = LL2MC[i];
                        break;
                    }
                }
            }
            return converter(lng, lat, cE);
        }

        Dictionary<string, float> converter(float x, float y, double[] cE)
        {
            double xTemp = cE[0] + cE[1] * Mathf.Abs(x);
            double cC = Mathf.Abs(y) / cE[9];
            double yTemp = cE[2] + cE[3] * cC + cE[4] * cC * cC + cE[5] * cC * cC * cC + cE[6] * cC * cC * cC * cC + cE[7] * cC * cC * cC * cC * cC + cE[8] * cC * cC * cC * cC * cC * cC;
            xTemp *= (x < 0 ? -1 : 1);
            yTemp *= (y < 0 ? -1 : 1);
            Dictionary<string, float> location = new Dictionary<string, float>();
            location.Add("x", (float)xTemp);
            location.Add("y", (float)yTemp);
            return location;
        }


        string CreateGetHttpResponse(string cityName, string ak)
        {
            string aaaa = "http://api.map.baidu.com/geocoder/v2/?address=" + cityName + "&output=json&ak=" + ak;
            HttpWebRequest request = WebRequest.Create(aaaa) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}