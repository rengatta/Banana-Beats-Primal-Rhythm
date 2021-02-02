using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailClickChecker : MonoBehaviour
{

    public BoxCollider2D boxCollider;
    public TextMeshProUGUI hitScoreText;
    public Smileys smileys;
    void Start()
    {
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            boxCollider.enabled = true;
            
            RaycastHit2D m_Hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up);
            if (m_Hit)
            {
                hitScoreText.text = "FAIL";
                smileys.ActivateSmiley(Smiley.Meh);
            }
        }
    }


}
