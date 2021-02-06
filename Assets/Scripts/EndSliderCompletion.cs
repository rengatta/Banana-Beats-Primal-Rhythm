using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSliderCompletion : MonoBehaviour
{

    public BoxCollider2D boxCollider;
    public bool isDetecting = false;

    public bool DetectHit()
    {
        //detects if the endslider is within the starting area of where the player "clicks" the hold slider
        int layerMask = ((1 << GlobalHelper.sliderLayer));
        RaycastHit2D[] m_Hit = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 0f, layerMask);

        bool hit = false;
        for(int i=0; i < m_Hit.Length; i++) {
            if (m_Hit[i].collider.gameObject.name == "right_end_slider") hit = true;
        }
        return hit;
    }

 




}
 
