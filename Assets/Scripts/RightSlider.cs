using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightSlider : MonoBehaviour, SliderInterface
{
    [HideInInspector]
    public bool can_destroy { get; set; } = true;

    public float horizontal_speed { get; set; }

    //if the slider is over the white triangle
    [HideInInspector]
    public bool can_click { get; set; }

    //the active slider is the leftmost slider that can still be clicked, prevents you from clicking multiple sliders at the same time
    [HideInInspector]
    public bool is_active { get; set; }

    [HideInInspector]
    public SliderInterface nextSlider { get; set; }

 
    public SliderType sliderType { get { return _sliderType; } set { _sliderType = value; } }
    [SerializeField]
    private SliderType _sliderType;

    Vector3 direction = Vector3.left;

    HitScore hitScore;

    public SpriteRenderer spriteRenderer;


    public void ActivateNextSlider()
    {
        nextSlider.can_click = true;
    }


    public void Darken()
    {
        Color tempColor = Color.black;
        tempColor.a = 0.4f;
        spriteRenderer.color = tempColor;
    }



    //makes sure the other sliders don't immediately trigger a click before the frame ends
    IEnumerator SetActive()
    {
        yield return new WaitForEndOfFrame();
        if (nextSlider != null)
        {
            nextSlider.is_active = true;
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        this.transform.position += (direction * horizontal_speed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.L) && can_click && is_active)
        {
            if (hitScore == HitScore.Perfect)
            {
                GlobalHelper.global.scoreManager.score += 10;
                GlobalHelper.global.scoreManager.combo += 1;
                GlobalHelper.global.hitScoreText.text = "PERFECT";
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Happy);
            }
            else if (hitScore == HitScore.Good)
            {
                GlobalHelper.global.scoreManager.score += 5;
                GlobalHelper.global.scoreManager.combo += 1;
                GlobalHelper.global.hitScoreText.text = "GOOD";
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Happy);
            }

            StartCoroutine(SetActive());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalHelper.perfectRegionLayer)
        {
            hitScore = HitScore.Perfect;
            can_click = true;

        }
        else if (collision.gameObject.layer == GlobalHelper.goodRegionLayer)
        {

            hitScore = HitScore.Good;
            can_click = true;

        }
    }

}
