using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;
//controls the pause menu logic
//mostly just controls what the buttons do
public class PauseMenu : MonoBehaviour
{
    public GameObject globalRoot;
    public GameObject pauseRoot;
    public GameObject optionsRoot;
    public GameObject instructionsRoot;


    public AudioSource audioSource;

    public Slider audioSlider;
    public SceneField levelSelectScene;
    public SceneField mainMenuScene;
    public SceneField retryScene;


    public Toggle muteToggle;

    public FadeInOut fadeInOut;

    public GameObject cogButton;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || GameState.androidMode == true) {
            cogButton.SetActive(true);
        } else {
            cogButton.SetActive(false);
        }
    }


    public void TogglePause() {
        if (GameState.paused)
        {
            GameState.paused = false;
            globalRoot.SetActive(false);
            pauseRoot.SetActive(false);
            optionsRoot.SetActive(false);
            Time.timeScale = 1.0f;
            GlobalHelper.global.audioSource.UnPause();
            AudioListener.pause = false;

        }
        else
        {
            GameState.paused = true;
            globalRoot.SetActive(true);
            pauseRoot.SetActive(true);
            Time.timeScale = 0.0f;
            GlobalHelper.global.audioSource.Pause();
            AudioListener.pause = true;
        }

    }

    public void UnPause() {
        GameState.paused = false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
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

    IEnumerator SceneFadeOut()
    {
        UnPause();
        SceneManager.LoadScene(levelSelectScene);
        yield return null;
    }

    IEnumerator MainMenuFadeOut()
    {
        UnPause();
        SceneManager.LoadScene(mainMenuScene);
        yield return null;
    }

    IEnumerator RetryFadeOut()
    {
        UnPause();
        SceneManager.LoadScene(retryScene);
        yield return null;
    }



    public void MainMenuButton() {
        fadeInOut.FadeOut(MainMenuFadeOut());
    }

    public void RetryButton() {

        fadeInOut.FadeOut(RetryFadeOut());
    }


    public void BackToLevelSelectButton() {

     
        fadeInOut.FadeOut(SceneFadeOut());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
}
