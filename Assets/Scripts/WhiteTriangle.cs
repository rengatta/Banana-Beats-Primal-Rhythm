using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes sure only one slider is triggered for every input press, analyzes each slider in the whitetriangle hitbox area
public class WhiteTriangle : MonoBehaviour
{

    public BoxCollider2D triangleRegion;

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
            
            if(sliderScript != null) {
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
        if(foundleft) {
            leftScript.DetectHit(); 
        }
        if(foundright) {
            rightScript.DetectHit();
        }

        

    }


    private void Update()
    {
        if(Input.anyKeyDown) {
            DetectHit();
        }
    }
}
