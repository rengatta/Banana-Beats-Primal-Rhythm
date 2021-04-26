#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

//contains functionality for recording notes during editing, saving levels, and loading levels
public class RecordManager : MonoBehaviour
{

    public Slider songDurationSlider;
    public TextMeshProUGUI recordFeedbackText;
    public TextMeshProUGUI recordFeedbackText2;
    public GameObject recordIcon;
    bool recording = false;
    public TextMeshProUGUI levelText;
    public GameObject recordRoot;
    public GameObject playTestRoot;
    public TextMeshProUGUI songNameText;
    bool isPlayTesting = false;
    public float saturation = 10;
    public AudioManager audioManager;
    public EditNotes editNotes;
    public SliderManager sliderManager;
    public Slider slider;
    public Transform leftParent;
    public Transform rightParent;
    public float tempCameraSize;
    public string currentSavePath = "";


    private void Start()
    {
        levelText.text = "";
    }

    public void PlayTestButtonClicked() {
        if(isPlayTesting) {
            StopAllCoroutines();
            editNotes.ToggleLines();
            isPlayTesting = false;
            playTestRoot.SetActive(false);
            recordRoot.SetActive(true);
            editNotes.Toggle();
            GlobalHelper.global.audioSource.Stop();
            GlobalHelper.global.audioSource.time = songDurationSlider.value;
            audioManager.playtestPaused = false;
            audioManager.Pause();
            audioManager.RestoreTimescale();
            sliderManager.DestroyActiveSliders();
            Camera.main.orthographicSize = tempCameraSize;

        } else {
            tempCameraSize = Camera.main.orthographicSize;
            Camera.main.orthographicSize = 5f;
            editNotes.ToggleLines();
            SaveEdit();
            isPlayTesting = true;
            playTestRoot.SetActive(true);
            editNotes.Toggle();
            recordRoot.SetActive(false);
            audioManager.ResetTimescale();
            audioManager.playtestPaused = true;
            sliderManager.PlayLevelRegular();
            GlobalHelper.global.audioSource.Stop();
            GlobalHelper.global.audioSource.clip = GlobalHelper.global.currentAudioClip;
            StartCoroutine(PreTimeWait());
            
        }
    }

    IEnumerator PreTimeWait() {
        yield return new WaitForSeconds(3.0f);
        GlobalHelper.global.audioSource.Stop();
        GlobalHelper.global.audioSource.time = 0.0f;
        GlobalHelper.global.audioSource.Play();
    }


    public void PlayTestCurrentTimeButtonClicked()
    {
        if (isPlayTesting)
        {
            StopAllCoroutines();
            editNotes.ToggleLines();
            isPlayTesting = false;
            playTestRoot.SetActive(false);
            recordRoot.SetActive(true);
            editNotes.Toggle();
            GlobalHelper.global.audioSource.Stop();
            GlobalHelper.global.audioSource.time = songDurationSlider.value;
            audioManager.playtestPaused = false;
            audioManager.Pause();
            audioManager.RestoreTimescale();
            sliderManager.DestroyActiveSliders();
            Camera.main.orthographicSize = tempCameraSize;

        }
        else
        {
            tempCameraSize = Camera.main.orthographicSize;
            Camera.main.orthographicSize = 5f;
            editNotes.ToggleLines();
            SaveEdit();
            isPlayTesting = true;
            playTestRoot.SetActive(true);
            editNotes.Toggle();
            recordRoot.SetActive(false);
            audioManager.ResetTimescale();
            audioManager.playtestPaused = true;
            sliderManager.PlayLevelAtTime(songDurationSlider.value);
            GlobalHelper.global.audioSource.clip = GlobalHelper.global.currentAudioClip;
            //GlobalHelper.global.audioSource.Stop();
            GlobalHelper.global.audioSource.time = songDurationSlider.value;
           
            GlobalHelper.global.audioSource.Play();

        }
    }




    public void LoadSong() {
        string path = EditorUtility.OpenFilePanel("Select an mp3 file", Application.dataPath + "\\Resources\\Audio\\", "mp3");

        if (path.Length != 0)
        {
            string songName = Path.GetFileNameWithoutExtension(path);
            GlobalHelper.global.currentAudioClip = Resources.Load<AudioClip>("Audio\\" + songName);
            songNameText.text = songName;
            GlobalHelper.global.currentLevel.audioClipName = songName;
            audioManager.Reset();
        }

    }

    public void LoadLevel() {
        string path = EditorUtility.OpenFilePanel("Select a level file", Application.dataPath + "\\Resources\\LevelSaves\\", "txt");

        if (path.Length == 0) {
            Debug.Log("File failed to load.");
            return;
        }
        currentSavePath = path;
        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
        } else {
            string readText = File.ReadAllText(path);

            GlobalHelper.global.currentLevel = JsonUtility.FromJson<LevelData>(readText);

            levelText.text = "Current Level: " + GlobalHelper.global.currentLevel.levelName;

                
            GlobalHelper.global.currentAudioClip = Resources.Load<AudioClip>("Audio\\" + GlobalHelper.global.currentLevel.audioClipName);
            songNameText.text = GlobalHelper.global.currentLevel.audioClipName;

        }
        editNotes.ResetLevelData();
        editNotes.LoadNotes();
        recordFeedbackText.text = "Level loaded.";
        recordFeedbackText2.text = System.DateTime.Now.ToString();
        audioManager.Reset();

    }

    public void SaveEdit() {
        GlobalHelper.global.currentLevel.ClearData();
        float speed = (float)GlobalHelper.global.currentLevel.sliderSpeed;

        foreach (Transform child in leftParent)
        {

            EditorSlider editorSlider = child.gameObject.GetComponent<EditorSlider>();
            EditorHoldSlider editorHoldSlider = child.gameObject.GetComponent<EditorHoldSlider>();
            if (editorSlider != null)
            {
                GlobalHelper.global.currentLevel.AddSlider(LevelSliderType.LeftSlider, editorSlider.transform.position.x / speed);

            } else if(editorHoldSlider != null) {
                int index = GlobalHelper.global.currentLevel.AddSlider(LevelSliderType.LeftHoldSlider, editorHoldSlider.start.transform.position.x / speed); ;
                GlobalHelper.global.currentLevel.AddHoldSliderEnd(index, (editorHoldSlider.start.transform.position.x / speed) + editorHoldSlider.length);

            }
        }

        foreach (Transform child in rightParent)
        {
            EditorSlider editorSlider = child.gameObject.GetComponent<EditorSlider>();
            EditorHoldSlider editorHoldSlider = child.gameObject.GetComponent<EditorHoldSlider>();

            if (editorSlider != null)
            {

                GlobalHelper.global.currentLevel.AddSlider(LevelSliderType.RightSlider, editorSlider.transform.position.x / speed);

            }
            else if (editorHoldSlider != null)
            {
                int index = GlobalHelper.global.currentLevel.AddSlider(LevelSliderType.RightHoldSlider, editorHoldSlider.start.transform.position.x / speed);
                GlobalHelper.global.currentLevel.AddHoldSliderEnd(index, (editorHoldSlider.start.transform.position.x / speed) + editorHoldSlider.length);

            }
        }
    }

    public void SaveLevel() {

        string path = EditorUtility.SaveFilePanel("Select a filename to save", Application.dataPath + "\\Resources\\LevelSaves\\", GlobalHelper.global.currentLevel.levelName, "txt");
        string levelname =  Path.GetFileNameWithoutExtension(path);

        if (path.Length == 0)
        {
            currentSavePath = "";
            Debug.Log("Save failed.");
            return;
        }

        currentSavePath = path;
        SaveEdit();
        GlobalHelper.global.currentLevel.levelName = levelname;

        string json = JsonUtility.ToJson(GlobalHelper.global.currentLevel);
        levelText.text = "Current Level: " + GlobalHelper.global.currentLevel.levelName;
        File.WriteAllText(path, json);
        recordFeedbackText.text = "Level saved.";
        recordFeedbackText2.text = System.DateTime.Now.ToString();
    }

    public void QuickSave() {
        if(currentSavePath != "") {

            SaveEdit();

            string json = JsonUtility.ToJson(GlobalHelper.global.currentLevel);

            File.WriteAllText(currentSavePath, json);
            recordFeedbackText.text = "Quicksaved to " + currentSavePath;
            recordFeedbackText2.text = System.DateTime.Now.ToString();
        } else {
            SaveLevel();
        }

    }

    public void RecordButtonClicked() {
        if(recording) {
            recording = false;
            recordIcon.SetActive(false);
        } else {
            recording = true;
            recordIcon.SetActive(true);
        }

    }

    bool holdLeft, holdRight = false;
    float holdLeftStart, holdRightStart;
    void Update()
    {
        if (recording && audioManager.songStarted)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                recordFeedbackText.text = "Left: " + songDurationSlider.value.ToString();
                editNotes.GenerateLeftNote(songDurationSlider.value * (float)GlobalHelper.global.currentLevel.sliderSpeed);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                recordFeedbackText2.text = "Right: " + songDurationSlider.value.ToString();
                editNotes.GenerateRightNote(songDurationSlider.value * (float)GlobalHelper.global.currentLevel.sliderSpeed);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                holdLeftStart = songDurationSlider.value;
                recordFeedbackText.text = "Hold left start: " + songDurationSlider.value.ToString();
                holdLeft = true;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                holdRightStart = songDurationSlider.value;
                recordFeedbackText2.text = "Hold right start: " + songDurationSlider.value.ToString();
                holdRight = true;
            }

            if (Input.GetKeyUp(KeyCode.Q) && holdLeft)
            {
                holdLeft = false;
                float length = songDurationSlider.value -holdLeftStart;
                editNotes.GenerateLeftHoldNote(holdLeftStart * (float)GlobalHelper.global.currentLevel.sliderSpeed, length * (float)GlobalHelper.global.currentLevel.sliderSpeed);
                recordFeedbackText.text = "Hold left end: " + songDurationSlider.value.ToString();
            }

            if (Input.GetKeyUp(KeyCode.O) && holdRight)
            {
                holdRight = false;
                float length = songDurationSlider.value - holdRightStart;
                editNotes.GenerateRightHoldNote(holdRightStart * (float)GlobalHelper.global.currentLevel.sliderSpeed, length * (float)GlobalHelper.global.currentLevel.sliderSpeed);
                recordFeedbackText2.text = "Hold right end: " + songDurationSlider.value.ToString();
            }
        }
    }


}
#endif