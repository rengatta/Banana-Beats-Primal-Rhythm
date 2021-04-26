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
    public static double songTimeDelta = 0f;
    public static bool failed = false;
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
    public static bool gameOptionsInit = false;
}

public class Global : MonoBehaviour
{

    
    public TextMeshProUGUI hitScoreText;
    public Smileys smileys;
    public ScoreManager scoreManager;
    public LevelData currentLevel;
    public AudioClip currentAudioClip;
    public AudioSource audioSource;
    public AudioSource effectsAudioSource;

    public GameObject hitScoreTextPrefab;





    public WhiteTriangle whiteTriangle;
    public bool inCharter = false;

    public float hitScoreTime;

    float sameTimeDistance = 80f;

    public void FailHit() {
        scoreManager.combo = 0;

        SpawnText("FAIL", Color.red + new Color(0.1f, 0.1f, 0.1f));
        whiteTriangle.ChangeColor(Color.red + new Color(0.1f, 0.1f, 0.1f));
        //hitScoreText.text = "FAIL";
        smileys.ActivateSmiley(Smiley.Angry);
        if(whiteTriangle.healthUI != null && !inCharter)
        whiteTriangle.healthUI.ChangeHP(-1);

    }
    public void SpawnText(string text, Color color) {
        float newTime = GlobalHelper.global.audioSource.time;
        if (Mathf.Abs(newTime - hitScoreTime) < 0.05f)
        {
            Instantiate(hitScoreTextPrefab).GetComponent<HitScoreText>().Init(text, GlobalHelper.global.hitScoreText.transform, color, sameTimeDistance);
        }
        else
        {
            Instantiate(hitScoreTextPrefab).GetComponent<HitScoreText>().Init(text, GlobalHelper.global.hitScoreText.transform, color, 0f);
        }
        hitScoreTime = newTime;

    }
    Color purple = new Color(75f / 255f, 0f / 255f, 130f / 255f);

    public void SpawnGoodText() {
        if (scoreManager.combo != 0 && scoreManager.combo % 10 ==0) {
            SpawnText("EXCELLENT", purple + new Color(0.1f, 0.1f, 0.1f));
            whiteTriangle.ChangeColor(purple + new Color(0.1f, 0.1f, 0.1f));
            whiteTriangle.healthUI.ChangeHP(1);
        }
        SpawnText("GOOD", Color.green + new Color(0.1f, 0.1f, 0.1f));
       whiteTriangle.ChangeColor(Color.green + new Color(0.1f, 0.1f, 0.1f));
    }

    public void SpawnPerfectText() {
        if (scoreManager.combo != 0 && scoreManager.combo % 10 == 0)
        {
            SpawnText("EXCELLENT", purple + new Color(0.1f, 0.1f, 0.1f));
            whiteTriangle.ChangeColor(purple + new Color(0.1f, 0.1f, 0.1f));
            whiteTriangle.healthUI.ChangeHP(1);
        }

        SpawnText("PERFECT", Color.cyan + new Color(0.1f, 0.1f, 0.1f));
        whiteTriangle.ChangeColor(Color.cyan + new Color(0.1f, 0.1f, 0.1f));
    }
    public void SpawnMissText() {
        SpawnText("MISS", Color.yellow + new Color(0.1f, 0.1f, 0.1f));
        whiteTriangle.ChangeColor(Color.yellow + new Color(0.1f, 0.1f, 0.1f));
    }

    int missHitCounter = 0;
    public void MissHit() {
        scoreManager.combo = 0;
        scoreManager.score -= 10;
        missHitCounter++;
        if(missHitCounter >= 3) {
            missHitCounter = 0;
            if (whiteTriangle.healthUI != null && !inCharter)
                whiteTriangle.healthUI.ChangeHP(-1);
            SpawnText("YOU'RE SLIPPING", Color.red + new Color(0.1f, 0.1f, 0.1f));
            whiteTriangle.ChangeColor(Color.red + new Color(0.1f, 0.1f, 0.1f));
        }
    }

    public void InitializeOptions()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume");
        }

        if (PlayerPrefs.HasKey("MuteToggled"))
        {
            int muteToggled = PlayerPrefs.GetInt("MuteToggled");
            if (muteToggled == 1)
            {
                audioSource.mute = true;
            }
            else
            {
                audioSource.mute = false;
            }
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") == 1)
            {
                Screen.fullScreen = true;

            }
        }

        if (PlayerPrefs.HasKey("Windowed"))
        {
            if (PlayerPrefs.GetInt("Windowed") == 1)
            {
                Screen.fullScreen = false;

            }
        }

    }

}
