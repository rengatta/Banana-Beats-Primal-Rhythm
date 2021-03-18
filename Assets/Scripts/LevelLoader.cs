using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
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


    public void InitializeLevel(string levelName) {
        songStarted = false;
        StopAllCoroutines();
        LoadLevel(levelName);
        
        sliderManager.PlayLevelDebug(debugStartTime, preTime);

        StartCoroutine(PreTimeWait());
    }


    IEnumerator PreTimeWait() {
        yield return new WaitForSeconds(preTime);


        audioSource.time = debugStartTime * audioSource.clip.length;
        audioSource.Play();
        songStarted = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (debugging)
        {
            InitializeLevel(debugLevelName);
        }
        else {

            if (SceneToSceneData.nextLevelName == "nolevel")
            {
                InitializeLevel(debugLevelName);

            }
            else
            {
                debugStartTime = 0.0f;
                InitializeLevel(SceneToSceneData.nextLevelName);
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
        SceneToSceneData.currentScore = GlobalHelper.global.scoreManager.score;
        SceneToSceneData.currentHighestCombo = GlobalHelper.global.scoreManager.highestCombo;
        SceneToSceneData.totalHits = GlobalHelper.global.scoreManager.totalHits;
        yield return new WaitForSeconds(endLevelPause);
        SceneManager.LoadScene(levelCompleteScene);
    }


    private void Update()
    {
        DetectLevelEnd();
    }



    public void LoadLevel(string levelName)
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = Application.dataPath + "\\LevelSaves\\" + levelName;

        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
            return;
        }
        else
        {
       
            string readText = File.ReadAllText(path);
            GlobalHelper.global.currentLevel = JsonUtility.FromJson<LevelData>(readText);
            GlobalHelper.global.currentAudioClip = Resources.Load<AudioClip>("Audio\\" + GlobalHelper.global.currentLevel.audioClipName);
            GlobalHelper.global.audioSource.clip = GlobalHelper.global.currentAudioClip;
        }


    }

}
