using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Utilities;
using System.IO;


//retrieves and calculates performance metrics to fill in the UI in the level complete scene
class PerformanceSaveData {
    public string levelName = "";
    public float bestScore = 0;
    public int bestCombo = 0;
    public string bestGrade = "F";
    public float maxPossibleScore = 1;
    public int maxPossibleCombo = 1;
    public float bestAccuracy = 0f;
    public PerformanceSaveData() {
        

    }

}

public class LevelComplete : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI accuracyText;
    public SceneField levelSelectScene;
    string grade = "F";

    public FadeInOut fadeInOut;


    void Start()
    {
        
        scoreText.text = "Score: " + (int)SceneToSceneData.currentScore + " / " + (int)SceneToSceneData.maxPossibleScore;
        comboText.text = "Highest Combo: " + SceneToSceneData.currentHighestCombo + " / " + SceneToSceneData.maxPossibleCombo;
        accuracyText.text = "Accuracy: " + System.Math.Round(((float)SceneToSceneData.totalHits / (float)SceneToSceneData.totalTries) * 100.0f, 2)  + "%";


        CalculateGrade();
        OverwritePerformanceData();
    }

    public void OverwritePerformanceData()
    {
        PerformanceSaveData previousSave;
        string path = Application.streamingAssetsPath + "\\PlayerPerformanceData\\" + SceneToSceneData.nextLevelName + ".txt";

        if (path.Length == 0)
        {
            return;
        }
        if (!File.Exists(path))
        {
            SaveNewLevel();
        }
        else
        {
            string readText = File.ReadAllText(path);

            previousSave = JsonUtility.FromJson<PerformanceSaveData>(readText);

            if(SceneToSceneData.currentScore > previousSave.bestScore) {
                previousSave.bestScore = SceneToSceneData.currentScore;
               
            }
            if (SceneToSceneData.currentHighestCombo > previousSave.bestCombo)
            {
                previousSave.bestCombo = SceneToSceneData.currentHighestCombo;
            }
            float currentAccuracy = ((float)SceneToSceneData.totalHits / (float)SceneToSceneData.totalTries) * 100.0f;
            if (currentAccuracy > previousSave.bestAccuracy) {
                previousSave.bestAccuracy = currentAccuracy;
                previousSave.bestGrade = grade;
            }

            string json = JsonUtility.ToJson(previousSave);
            File.WriteAllText(path, json);
        }


    }


    public void SaveNewLevel()
    {

        string path = Application.streamingAssetsPath + "\\PlayerPerformanceData\\" + SceneToSceneData.nextLevelName + ".txt";

        PerformanceSaveData newSave = new PerformanceSaveData();
        newSave.levelName = SceneToSceneData.nextLevelName;
        newSave.bestScore = SceneToSceneData.currentScore;
        newSave.bestCombo = SceneToSceneData.currentHighestCombo;
        newSave.bestAccuracy = ((float)SceneToSceneData.totalHits / (float)SceneToSceneData.totalTries) * 100.0f;
        newSave.bestGrade = grade;

        string json = JsonUtility.ToJson(newSave);
        File.WriteAllText(path, json);
    }


    void CalculateGrade() {

        float gradePercentage = ((float)SceneToSceneData.currentScore / (float)SceneToSceneData.maxPossibleScore);

        if(gradePercentage >= 0.9f) {
            gradeText.text = "Grade: S";
            grade = "S";

        } else if(gradePercentage >= 0.8f) {
            gradeText.text = "Grade: A";
            grade = "A";
        }
        else if (gradePercentage >= 0.7f) {
            gradeText.text = "Grade: B";
            grade = "B";
        }
        else if (gradePercentage >= 0.6f) {
            gradeText.text = "Grade: C";
            grade = "C";
        }
        else if (gradePercentage >= 0.5f) {
            gradeText.text = "Grade: D";
            grade = "D";
        }   
        else if (gradePercentage < 0.5f) {
            gradeText.text = "Grade: F";
            grade = "F";
        }

    }

    IEnumerator FadeOut() {
        SceneManager.LoadScene(levelSelectScene);
        yield return null;
    }


    public void GotoLevelSelect() {
        fadeInOut.FadeOut(FadeOut());
    }
    
}
