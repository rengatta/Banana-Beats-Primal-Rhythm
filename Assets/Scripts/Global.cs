using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//helps access the global singleton and stores slider layer identifiers
//probably a bad idea to put any more variables in global
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
    public static int editorSliderLayer = 14;
    public static int editorStripLayer = 15;

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

}


public class Global : MonoBehaviour
{
    public TextMeshProUGUI hitScoreText;
    public Smileys smileys;
    public ScoreManager scoreManager;
    public LevelData currentLevel;
    public AudioClip currentAudioClip;
    public AudioSource audioSource;
    public WhiteTriangle whiteTriangle;
}
