using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Utilities;
using System.IO;
using UnityEngine.UI;
//controls the logic in the level select menu
//fills each level UI with the corresponding performance data
public class LevelSelectLevel : MonoBehaviour
{
    public TextMeshProUGUI songNameText;
    public string songName = "";
    public TextMeshProUGUI yourBestScore;
    public TextMeshProUGUI yourBestGrade;
    public TextMeshProUGUI yourBestCombo;
    public TextMeshProUGUI yourBestAccuracy;
    public string levelFilename = "";
    public SceneField songScene;


    public FadeInOut fadeInOut;


    private void OnValidate()
    {
        songNameText.text = songName;
    }

    private void Start()
    {
        FillPerformanceData();
    }
    void FillPerformanceData() {

        string path = Application.streamingAssetsPath + "\\PlayerPerformanceData\\" + levelFilename + ".txt";

        if (!File.Exists(path))
        {
            yourBestScore.text = "Best Score: X";
            yourBestGrade.text = "Best Grade: X";
            yourBestCombo.text = "Best Combo: X";
            yourBestAccuracy.text = "Best Accuracy: X";
            return;
        }


        string readText = File.ReadAllText(path);

        PerformanceSaveData performanceData = JsonUtility.FromJson<PerformanceSaveData>(readText);

        yourBestScore.text = "Best Score: " + (int)performanceData.bestScore; 
        yourBestGrade.text = "Best Grade: " + performanceData.bestGrade;
        yourBestCombo.text = "Best Combo: " + performanceData.bestCombo;
        yourBestAccuracy.text = "Best Accuracy: " + System.Math.Round(performanceData.bestAccuracy, 2) + "%";
    }


    public void PlayButtonClicked() {
        SceneToSceneData.nextLevelName = levelFilename;
        fadeInOut.FadeOut(LoadLevel());
    }

    IEnumerator LoadLevel() {
        SceneManager.LoadScene(songScene);
        yield return null;
    }



}
