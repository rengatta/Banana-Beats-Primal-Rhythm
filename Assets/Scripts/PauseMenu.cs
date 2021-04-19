using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;

public class PauseMenu : MonoBehaviour
{
    public GameObject globalRoot;
    public GameObject pauseRoot;
    public GameObject optionsRoot;
    public GameObject instructionsRoot;


    public AudioSource audioSource;

    public Slider audioSlider;

    public SceneField levelSelectScene;

    public Toggle muteToggle;

    public void TogglePause() {
        if (GameState.paused)
        {
            GameState.paused = false;
            globalRoot.SetActive(false);
            pauseRoot.SetActive(false);
            optionsRoot.SetActive(false);
            Time.timeScale = 1.0f;
            audioSource.UnPause();

        }
        else
        {
            GameState.paused = true;
            globalRoot.SetActive(true);
            pauseRoot.SetActive(true);
            Time.timeScale = 0.0f;
            audioSource.Pause();
        }

    }


    public void OptionsMenuButton() {
        pauseRoot.SetActive(false);
        optionsRoot.SetActive(true);
    }

    public void OptionsMenuBackButton() {
        pauseRoot.SetActive(true);
        optionsRoot.SetActive(false);

    }

    public void InstructionsButtonPressed() {
        instructionsRoot.SetActive(true);
        pauseRoot.SetActive(false);

    }

    public void InstructionsBackButtonPressed()
    {
        instructionsRoot.SetActive(false);
        pauseRoot.SetActive(true);

    }


    public void BackToLevelSelectButton() {
        GameState.paused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(levelSelectScene);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
}
