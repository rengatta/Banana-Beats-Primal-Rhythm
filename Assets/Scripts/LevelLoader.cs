using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
//used in the level gameplay scene to load in the level that was selected in the levelselect menu
public class LevelLoader : MonoBehaviour
{
    public SliderManager sliderManager;

    public bool debugging = true;
    public string debugLevelName;
    [Range(0.0f, 1.0f)]
    public float debugStartTime = 0.0f;
    public AudioSource audioSource;
    bool songStarted = false;
    public SceneField levelCompleteScene;
    public float endLevelPause = 2f;
    public float preTime = 3.0f;

    public FadeInOut fadeInOut;


    public void InitializeLevel2(string levelName)
    {
        songStarted = false;
        StopAllCoroutines();
        LoadLevel(levelName);

        sliderManager.PlayLevelRegular(preTime);
        songStarted = true;
    }



    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  
        Application.targetFrameRate = 144;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (debugging)
        {
            InitializeLevel2(debugLevelName);
        }
        else {

            if (SceneToSceneData.nextLevelName == "nolevel")
            {
                InitializeLevel2(debugLevelName);
                //InitializeLevel2(debugLevelName);

            }
            else
            {
                debugStartTime = 0.0f;
                InitializeLevel2(SceneToSceneData.nextLevelName);
            }

        }


    }


    public void DetectLevelEnd()
    {
        if (songStarted)
        {
            if (audioSource.time >= audioSource.clip.length)
            {
                StartCoroutine(GotoLevelCompleteScene());
            }
        }
    }

    IEnumerator GotoLevelCompleteScene() {
        if (GameState.failed != true)
        {
            SceneToSceneData.currentScore = GlobalHelper.global.scoreManager.score;
            SceneToSceneData.currentHighestCombo = GlobalHelper.global.scoreManager.highestCombo;
            SceneToSceneData.totalHits = GlobalHelper.global.scoreManager.totalHits;
            yield return new WaitForSeconds(endLevelPause);



            fadeInOut.FadeOut(FadeOutLoad()); 
        }
    }

    IEnumerator FadeOutLoad() {
        SceneManager.LoadScene(levelCompleteScene);
        yield return null;
    }

    private void Update()
    {
        DetectLevelEnd();
    }


    public void LoadLevel(string levelName)
    {
        TextAsset mydata = Resources.Load<TextAsset>("LevelSaves\\" + levelName);
        string readText = mydata.text;
        GlobalHelper.global.currentLevel = JsonUtility.FromJson<LevelData>(readText);
        GlobalHelper.global.currentAudioClip = Resources.Load<AudioClip>("Audio\\" + GlobalHelper.global.currentLevel.audioClipName);
        GlobalHelper.global.audioSource.clip = GlobalHelper.global.currentAudioClip;
        
    }

}
