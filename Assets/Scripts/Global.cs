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


public static class GameState
{
    public static bool paused = false;
}

public static class SceneToSceneData
{
    static SceneToSceneData()
    {

    }
    public static float holdSliderScore = 5f;
    public static float holdSliderHoldScore = 0.1f;
    public static float sliderScore = 10f;
    public static float perfectMultiplier = 1.2f;
    public static float maxPossibleScore = 1f;
    public static int maxPossibleCombo = 1;
    public static float currentScore = 0f;
    public static int currentHighestCombo = 0;
    public static string nextLevelName = "nolevel";
    public static int totalHits = 0;
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
    public bool inCharter = false;

    public void FailHit() {
        scoreManager.combo = 0;
        hitScoreText.text = "FAIL";
        smileys.ActivateSmiley(Smiley.Angry);
        if(whiteTriangle.healthUI != null && !inCharter)
        whiteTriangle.healthUI.ChangeHP(-1);

    }
}
