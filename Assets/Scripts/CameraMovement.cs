using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//moves along the editor strip when the background song is playing
public class CameraMovement : MonoBehaviour
{
    float movementSpeed = 5f;
    public GameObject backgroundStrips;
    bool moving = true;
    public Slider slider;

    public void StartMoving() {
        if (GlobalHelper.global.currentLevel != null) movementSpeed = (float)GlobalHelper.global.currentLevel.sliderSpeed;
        backgroundStrips.SetActive(true);
        moving = true;

    }
    public void StopMoving() {
        moving = false;
    }

    private void Update()
    {
        if (moving)
            this.transform.position = new Vector3(slider.value * movementSpeed, this.transform.position.y, -10f);
    }

    public void ResetCamera()
    {
        moving = false;
        backgroundStrips.SetActive(false);
        this.transform.position = new Vector3(0f, 0f, -10f);
    }


}
