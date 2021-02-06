using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharterManager : MonoBehaviour
{

    public AudioClip currentSong;
    public AudioSource audioSource;
    public List<Bars> songBars;
    public TextMeshProUGUI songDurationText;
    public float currentSongDuration;
    public string chartName = "";
    bool songPaused = true;
    bool songStarted = false;
    public Slider slider;


    public class Bars {
        public float timeToReachTriangle { get; set; }
        public float speed { get; set; }
        public float timeToSpawn { get; set; }
    }

    void LoadChartData() {



    }

    void RenderSongWaveforms() {
  
   
    }

    IEnumerator StartSong() {
        audioSource.clip = currentSong;
        audioSource.Stop();
        audioSource.Play();
        slider.maxValue = audioSource.clip.length -0.1f;
        songPaused = false;

        while(true) {
            
            if(!songPaused) {
                RenderSongWaveforms();
            }
            yield return null;
        }

    }

    IEnumerator UpdateSongDuration() {

        while (true)
        {

            if (!songPaused)
            {
                songDurationText.text = string.Format("{0:N2}", currentSongDuration);
                slider.value = currentSongDuration;
                currentSongDuration += Time.deltaTime;
                
                if(currentSongDuration >= audioSource.clip.length - 0.1f) {
                    audioSource.time = 0f;
                    currentSongDuration = 0f;

                }
            }
            yield return null;
        }

    }

    public void StartButtonPressed() {

        StopAllCoroutines();
        currentSongDuration = 0f;
        songPaused = true;
        StartCoroutine(StartSong());
        StartCoroutine(UpdateSongDuration());
    }


    public void PauseButtonPressed() {
        audioSource.Pause();
        songPaused = true;
    }

    public void ResumeButtonPressed()
    {
        if (!songStarted)
        {
            audioSource.clip = currentSong;
            audioSource.Stop();
            audioSource.Play();
            slider.maxValue = audioSource.clip.length - 0.1f;
            songPaused = false;
            songStarted = true;
            StopAllCoroutines();
            StartCoroutine(UpdateSongDuration());

        }
        else
        {


            audioSource.UnPause();
            songPaused = false;
        }
    }


    public void SliderClicked() {
        audioSource.Pause();
        songPaused = true;


    }

    public void SliderEndDrag() {
        currentSongDuration = slider.value;
        audioSource.time = slider.value;
        audioSource.UnPause();
        songPaused = false;


    }


}   



