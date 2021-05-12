using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//added to sound effect audiosources
//sets ignoreListenerPause to true so that button sound effects will play in the pause menu
[RequireComponent(typeof(AudioSource))]
public class AudioSourceSettings : MonoBehaviour
{
    public bool ignoreListenerPause = false;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = ignoreListenerPause;
    }

}
