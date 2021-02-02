using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HorizontalSlider : MonoBehaviour
{

    public float horizontal_speed = 10f;

    //if the slider is over the white triangle
    public bool can_click = false;

    //the active slider is the leftmost slider that can still be clicked, prevents you from clicking multiple sliders at the same time
    public bool is_active = false;

    [HideInInspector]
    public Smileys smileys;

    public HorizontalSlider nextSlider;

    enum HitScore
    {
        Fail,
        Perfect,
        Good
    }

    HitScore hitScore;

    [HideInInspector]
    public TextMeshProUGUI hitScoreText;

    public void ActivateNextSlider()
    {
        nextSlider.can_click = true;
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
        this.transform.position += (Vector3)(Vector2.left * horizontal_speed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && can_click && is_active)
        {
            if (hitScore == HitScore.Perfect)
            {
                hitScoreText.text = "PERFECT";
                smileys.ActivateSmiley(Smiley.Happy);
            }
            else if (hitScore == HitScore.Good)
            {
                hitScoreText.text = "GOOD";
                smileys.ActivateSmiley(Smiley.Happy);
            }

            StartCoroutine(SetActive());

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            hitScore = HitScore.Perfect;
            can_click = true;

        }
        else if (collision.gameObject.layer == 11)
        {

            hitScore = HitScore.Good;
            can_click = true;

        }
    }

}
