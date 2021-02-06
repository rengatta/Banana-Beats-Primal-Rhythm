using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public static class GlobalHelper
{

    static GlobalHelper()
    {

        GameObject globalInstance = GameObject.Find("Global");
        _global = globalInstance.GetComponent<Global>();

    }

    public static int sliderLayer = 6;
    public static int whiteTriangleLayer = 7;
    public static int sliderDespawnerLayer = 8;
    public static int inactiveSliderRegionLayer = 9;
    public static int perfectRegionLayer = 10;
    public static int goodRegionLayer = 11;
   

    public static TextMeshProUGUI test;


    private static Global _global;
    public static Global global
    {
        get
        {

            if (_global == null)
            {
                _global = GameObject.Find("Global").GetComponent<Global>();
            }

            return _global;
        }
    }

    /*
    public static GlobalUI globalUI
    {
        get { return GameObject.Find("Global").GetComponent<GlobalUI>(); }
    }
    

    public static Transform canvas
    {
        get { return GameObject.Find("Canvas").transform; }
    }

    public static Vector3 worldCoordsMousePosition
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }
    */
}


public class Global : MonoBehaviour
{
    public TextMeshProUGUI hitScoreText;
    public Smileys smileys;
    public ScoreManager scoreManager;
}
