#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//class for storing most of the input events in one place for organization
public class LevelEditorInput : MonoBehaviour
{

    public EditNotes editNotes;
    public AudioManager audioManager;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            audioManager.ResumeButtonPressed();

        }


        if (Input.GetMouseButtonDown(0))
        {
            editNotes.MouseButtonDown();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            editNotes.DeleteKey();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            editNotes.Alpha1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            editNotes.Alpha2();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {

            if (Input.GetKeyDown(KeyCode.S))
            {
                editNotes.LeftControlSave();
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) // forward
            {
                editNotes.ScrollWheelUp();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) // backwards
            {
                editNotes.ScrollWheelDown();
            }


        } else {

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) // forward
            {
                audioManager.ScrollWheelUp();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) // backwards
            {
                audioManager.ScrollWheelDown();
            }


        }


    }
}
#endif