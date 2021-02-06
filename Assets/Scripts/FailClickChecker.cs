using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailClickChecker : MonoBehaviour
{
    //actually checks for a "MISS"

    public BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.L)) {
            boxCollider.enabled = true;

            int layerMask = ((1 << GlobalHelper.sliderLayer));
            RaycastHit2D m_Hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 0f, layerMask);


            if (!m_Hit)
            {
                GlobalHelper.global.hitScoreText.text = "MISS";
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Meh);
            } else {
                if (m_Hit.collider.gameObject.layer == GlobalHelper.sliderLayer)
                {

                    SliderInterface sliderInterface = m_Hit.collider.gameObject.GetComponent<SliderInterface>();

                    if (sliderInterface != null)
                    {

                        if (sliderInterface.sliderType == SliderType.RightSlider && Input.GetKeyDown(KeyCode.A))
                        {
                            //GlobalHelper.global.scoreManager.combo = 0;
                            GlobalHelper.global.hitScoreText.text = "MISS";
                            GlobalHelper.global.smileys.ActivateSmiley(Smiley.Meh);
                        }
                        else if (sliderInterface.sliderType == SliderType.LeftSlider && Input.GetKeyDown(KeyCode.L))
                        {
                            //GlobalHelper.global.scoreManager.combo = 0;
                            GlobalHelper.global.hitScoreText.text = "MISS";
                            GlobalHelper.global.smileys.ActivateSmiley(Smiley.Meh);
                        }
                    }

                }
            }


        }

    }


}
