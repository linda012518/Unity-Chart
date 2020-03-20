using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.UI;

public class city : MonoBehaviour {

    CityPosition pos;

    public Image map;
    public RectTransform cityPoint;

    public string province;
    public string cityName;

	// Use this for initialization
	void Start () {

        pos = new CityPosition();

        pos.SetMapAndCity(map, cityPoint);
        pos.Init(province);
        pos.SetCityPos(cityName);



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
