using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSliderCompletion : MonoBehaviour
{
    [HideInInspector]
    public GameObject endSlider;

    public BoxCollider2D boxCollider;

    public bool DetectHit()
    {
        //detects if the endslider is within the starting area of where the player "clicks" the hold slider
        int layerMask = ((1 << GlobalHelper.sliderLayer));
        RaycastHit2D[] m_Hit = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 0f, layerMask);

        for(int i=0; i < m_Hit.Length; i++) {
            if (m_Hit[i].collider.gameObject == endSlider) return true;
        }
        return false;
    }

 




}
 
