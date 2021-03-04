using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//manages the audio in the level editor
public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public List<Bars> songBars;
    public TextMeshProUGUI songDurationText;
    public float currentSongDuration;
    public string chartName = "";
    bool songPaused = true;
    public bool songStarted = false;
    public Slider slider;
    public CameraMovement cameraMovement;
    bool sliderPaused = false;
    public float mouseWheelSpeed = 0.2f;
    public bool playtestPaused = false;
    public Slider volumeSlider;
    public Slider timeScaleSlider;

    public void OnVolumeChanged()
    {
        audioSource.volume = volumeSlider.value;

    }


    public void ChangeTimescale()
    {
        audioSource.Pause();
        Time.timeScale = timeScaleSlider.value;
        audioSource.pitch = Time.timeScale;
        if (songPaused != true)
            audioSource.UnPause();
    }

    public void ResetTimescale() {
        audioSource.Pause();
        timeScaleSlider.value = 1f;
        Time.timeScale = 1f;
        audioSource.pitch = 1f;
        if (songPaused != true)
            audioSource.UnPause();
    }

    public class Bars {
        public float timeToReachTriangle { get; set; }
        public float speed { get; set; }
        public float timeToSpawn { get; set; }
    }

    IEnumerator UpdateSongDuration() {

        while (true)
        {
            if (!songPaused && !sliderPaused)
            {
              
                slider.value = currentSongDuration;
                currentSongDuration += Time.deltaTime;
                
                if(currentSongDuration >= audioSource.clip.length - 0.1f) {
                    audioSource.time = 0f;
                    currentSongDuration = 0f;

                }
            }

            songDurationText.text = string.Format("{0:N2}", slider.value);

            yield return null;
        }

    }


    public void Reset()
    {

        audioSource.clip = GlobalHelper.global.currentAudioClip;
        audioSource.Stop();
        audioSource.Play();
        slider.maxValue = audioSource.clip.length - 0.1f;
        currentSongDuration = 0.0f;
        songPaused = false;
        songStarted = true;
        StopAllCoroutines();
        StartCoroutine(UpdateSongDuration());

    }

    public void Pause() {
        audioSource.Pause();
        songPaused = true;
    }

    public void ResumeButtonPressed()
    {

        if (GlobalHelper.global.currentAudioClip == null) return;


        if (!songStarted) {
            Reset();
        }
        else {
            if (!songPaused) {
                Pause();
            }
            else {
                audioSource.time = slider.value;
                audioSource.Play();
                songPaused = false;
            }
        }
        
    }


    public void SliderClicked() {
        if (songPaused != true)
            audioSource.Pause();
        sliderPaused = true;
    }


    public void SliderEndDrag() {
        currentSongDuration = slider.value;
        audioSource.time = slider.value;
        sliderPaused = false;
        if(songPaused != true)
            audioSource.UnPause();
    }


    public void ScrollWheelUp() {
        if (playtestPaused) return;
        if (songPaused != true)
            audioSource.Pause();
        if ((slider.value + mouseWheelSpeed) > slider.maxValue) slider.value = slider.maxValue;
        else
            slider.value += mouseWheelSpeed;
        currentSongDuration = slider.value;
        audioSource.time = slider.value;
        if (songPaused != true)
            audioSource.UnPause();
    }


    public void ScrollWheelDown() {
        if (playtestPaused) return;
        if (songPaused != true)
            audioSource.Pause();
        if ((slider.value - mouseWheelSpeed) < 0.0f) slider.value = 0.0f;
        else
            slider.value -= mouseWheelSpeed;
        currentSongDuration = slider.value;
        audioSource.time = slider.value;
        if (songPaused != true)
            audioSource.UnPause();

    }

}   



