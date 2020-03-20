using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class PolygonSlider_Example : MonoBehaviour
{

    #region PolygonSlider

    public PolygonSlider polygonSlider1;
    public PolygonSlider polygonSlider2;

    #endregion


    #region NumberOrder

    public NumberOrder numberOrder;

    #endregion

    public Button reset;

    void Start()
    {

        #region PolygonSlider

        polygonSlider1.EnterAnim(0.66f);
        polygonSlider2.EnterAnim(0.8f);

        #endregion

        reset.onClick.AddListener(delegate ()
        {
            #region PolygonSlider

            polygonSlider1.EnterAnim(0.75f);
            polygonSlider2.EnterAnim(0.45f);

            #endregion
        });

        #region NumberOrder

        numberOrder.NewNumber = 4896267;
        StartCoroutine(updateNumber());

        #endregion

    }

    #region NumberOrder

    IEnumerator updateNumber()
    {
        while (true)
        {
            numberOrder.NewNumber += 21;
            yield return new WaitForSeconds(1);
        }

    }

    #endregion



}
