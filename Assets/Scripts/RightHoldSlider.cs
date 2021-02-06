using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum HitScore
{
    Fail,
    Perfect,
    Good
}

public class RightHoldSlider : MonoBehaviour, SliderInterface
{

    [HideInInspector]
    public bool can_destroy { get; set; } = true;

    public float length = 10f;

    public EndSliderCompletion endSliderCompletion;

    public Transform body;
    public SpriteRenderer bodySprite;
    public Transform endSlider;
    public SpriteRenderer startSliderSpriteRenderer;
    public SpriteRenderer endSliderSpriteRenderer;
    public Transform start;


    public GameObject spriteMaskRoot;
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

    HitScore hitScore;


    Vector3 direction = Vector3.left;

    bool clicked = false;

    void Start()
    {
        start.transform.localPosition += new Vector3(bodySprite.bounds.size.x / 2f, 0f, 0f);
        body.localScale = new Vector3(length, body.localScale.y);

        float xSize = bodySprite.bounds.size.x;
        endSlider.position = new Vector3(start.transform.position.x + xSize, endSlider.position.y, endSlider.position.z);
    }




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
        
    }

    void Update()
    {
        start.transform.position += (direction * horizontal_speed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.L) && can_click && is_active && !clicked)
        {
            clicked = true;
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

            StartCoroutine(HoldDown());
            StartCoroutine(SetActive());

        }
    }


    public float scoreModifier = 0.1f;

    IEnumerator HoldDown() {
        can_destroy = false;
        //instantiates the sprite mask gameobject and sets it to the parent of this gameobject so that it won't mask any other sprites outside
        GameObject spriteMaskRootInstance = Instantiate(spriteMaskRoot);
        spriteMaskRootInstance.transform.position = this.transform.position;
        this.transform.parent = spriteMaskRootInstance.transform;
        //set it to this transform's x position
        spriteMaskRoot.transform.position = new Vector3(this.transform.position.x, spriteMaskRoot.transform.position.y, spriteMaskRoot.transform.position.z);

        //removes the child that contains the box collider so we can detect the end slider later
        endSliderCompletion.gameObject.transform.parent = null;
  
        while(true) {
            GlobalHelper.global.scoreManager.score += scoreModifier * Time.deltaTime;

            //detects if the player lets go of a hold in order to "catch" the end slider
            if(Input.GetKeyUp(KeyCode.L)) {
                //detach from the sprite mask root since we need the sprite mask to move with the slider now
                this.transform.parent = null;
                //makes sure the sprite mask only works on children of the hold slider; SortinGroup component needs to be added to the root parent for this to function
                gameObject.AddComponent(typeof(SortingGroup)); //as SortingGroup;

                //parent the sprite mask to the slider so it moves with the slider
                Transform spriteMaskChild = spriteMaskRootInstance.transform.GetChild(0);
                spriteMaskChild.SetParent(this.transform);

                //destroy the sprite mask's previous root since we don't need it anymore
                Destroy(spriteMaskRootInstance);
                

                if (nextSlider != null)
                    nextSlider.is_active = true;

                if (endSliderCompletion.DetectHit())
                {
                    GlobalHelper.global.scoreManager.score += 5;
                    GlobalHelper.global.scoreManager.combo += 1;
                    Destroy(endSliderCompletion.gameObject);
                    Destroy(this.gameObject);
                } else {
                    GlobalHelper.global.scoreManager.combo = 0;
                }

                Darken();

                break;
            }
            yield return null;
        }
    }

 

    public void Darken() {
        float tempAlpha = bodySprite.color.a;
        Color tempColor = Color.black;
        tempColor.a = tempAlpha;

        bodySprite.color = tempColor;

        tempColor = Color.black;
        tempColor.a = 0.6f;


        endSliderSpriteRenderer.color = tempColor;

        startSliderSpriteRenderer.color = tempColor;

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
