using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes sure only one slider is triggered for every input press, analyzes each slider in the whitetriangle hitbox area
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


    public void DetectHit() {
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



        if (foundleft || foundright)
        {

            if (foundleft)
                leftScript.DetectHit();
            if (foundright)
                rightScript.DetectHit();
            if (foundleft && foundright) return;

            if (foundleft) {

                if (Input.GetKeyDown(KeyCode.L)) {
                    Miss();
                }
            }
            else if (foundright) {

                if (Input.GetKeyDown(KeyCode.A)) {
                    Miss();
                }

            }

        }
        else {
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.A)) {
                Miss();
            }
        }

        

    }


    void Miss() {

        GlobalHelper.global.SpawnMissText();
        //Instantiate(GlobalHelper.global.hitScoreTextPrefab).GetComponent<HitScoreText>().Init("MISS", GlobalHelper.global.hitScoreText.transform);
        GlobalHelper.global.smileys.ActivateSmiley(Smiley.Meh);
        GlobalHelper.global.MissHit();
    }

    private void Update()
    {
        if(Input.anyKeyDown) {
            DetectHit();
        }
    }
}
