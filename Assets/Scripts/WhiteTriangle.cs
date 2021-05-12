using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//makes sure only one slider is triggered for every input press, analyzes each slider in the whitetriangle hitbox area
//also sends information to the slider itself, that it is being recieved
public class WhiteTriangle : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public BoxCollider2D triangleRegion;
    public HealthUI healthUI;

    float stayColorTime = 0.1f;
    float revertColorSpeed = 5f;

    public void ChangeColor(Color color) {
        StopAllCoroutines();
        spriteRenderer.color = color;
        StartCoroutine(RevertColor());
    }

    IEnumerator RevertColor() {
        yield return new WaitForSeconds(stayColorTime);
        float r, g, b = g = r = 0.0f;


        while(spriteRenderer.color != Color.white) {
            r = Mathf.Clamp(spriteRenderer.color.r + revertColorSpeed * Time.deltaTime, 0.0f, 1.0f);
            g = Mathf.Clamp(spriteRenderer.color.g + revertColorSpeed * Time.deltaTime, 0.0f, 1.0f);
            b = Mathf.Clamp(spriteRenderer.color.b + revertColorSpeed * Time.deltaTime, 0.0f, 1.0f);

            spriteRenderer.color = new Color(r, g, b);
            yield return null;
        }
   

    }


    public void DetectHit(bool mouseDown) {
        //detects if the endslider is within the starting area of where the player "clicks" the hold slider
        int layerMask = ((1 << GlobalHelper.sliderLayer));
        RaycastHit2D[] m_Hit = Physics2D.BoxCastAll(triangleRegion.bounds.center, triangleRegion.bounds.size, 0f, Vector2.up, 0f, layerMask);
        SliderInterface sliderScript;
        bool foundleft = false;
        float leftxposition = 0f;
        SliderInterface leftScript = null;
        bool foundright = false;
        float rightxposition = 0f;
        SliderInterface rightScript = null;

        //makes sure that multiple sliders aren't activated in a single keypress. leftmost or rightmost sliders take activation precedence depending on the direction
        for (int i = 0; i < m_Hit.Length; i++)
        {
            sliderScript = m_Hit[i].collider.gameObject.GetComponent<SliderInterface>();
            
            if(sliderScript != null && sliderScript.hit != true) {
                if (sliderScript.sliderType == SliderType.LeftSlider)
                {
                    if(!foundleft) {
                        
                        leftxposition = m_Hit[i].collider.transform.position.x;
                        leftScript = sliderScript;
                        foundleft = true;
                    } else {
                        if(m_Hit[i].collider.transform.position.x > leftxposition) {
                            leftxposition = m_Hit[i].collider.transform.position.x;
                            leftScript = sliderScript;
                        }
                    }
                }
                else if(sliderScript.sliderType == SliderType.RightSlider)
                {
                    if (!foundright)
                    {
                        rightxposition = m_Hit[i].collider.transform.position.x;
                        rightScript = sliderScript;
                        foundright = true;
                    }
                    else
                    {
                        if (m_Hit[i].collider.transform.position.x < rightxposition)
                        {
                            rightxposition = m_Hit[i].collider.transform.position.x;
                            rightScript = sliderScript;
                        }
                    }

                }
            }
         
        }

        ClickDirection clickDirection = ClickDetector.GetClickDirection(mouseDown);


        if (foundleft || foundright)
        {
            //activate sliders
            if (foundleft)
                leftScript.DetectHit(clickDirection);
            if (foundright)
                rightScript.DetectHit(clickDirection);
            //next, check for "miss" presses. don't have to check if both types of sliders are in the triangle
            if (foundleft && foundright) return;

      

            if (foundleft) {
                //left side but right button pressed
                if (clickDirection == ClickDirection.right) {
                    Miss();
                }
            }
            else if (foundright) {
                //right side but left button pressed
                if (clickDirection == ClickDirection.left) {
                    Miss();
                }

            }

        }
        else {
            //nothing is in the triangle so there is always a miss regardless of which key is pressed
            if (clickDirection == ClickDirection.left || clickDirection == ClickDirection.right) {
                Miss();
            }
        }

        

    }



    void Miss() {

        GlobalHelper.global.SpawnMissText();
        GlobalHelper.global.smileys.ActivateSmiley(Smiley.Meh);
        GlobalHelper.global.MissHit();
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.L)) {
            DetectHit(false);
        }
    }
}
