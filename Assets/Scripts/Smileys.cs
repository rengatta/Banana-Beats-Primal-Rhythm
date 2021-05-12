using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Smiley {
    Happy,
    Angry,
    Meh

}
//controls the monkey expressions that appear in the lower left corner

public class Smileys : MonoBehaviour
{
    public GameObject smileyHappy;
    public GameObject smileyMeh;
    public GameObject smileyAngry;


    public void ActivateSmiley(Smiley smiley) {
        if(smiley == Smiley.Happy) {
            smileyHappy.SetActive(true);
            smileyMeh.SetActive(false);
            smileyAngry.SetActive(false);
        } else if (smiley == Smiley.Angry) {
            smileyHappy.SetActive(false);
            smileyMeh.SetActive(false);
            smileyAngry.SetActive(true);
        } else if (smiley == Smiley.Meh) {
            smileyHappy.SetActive(false);
            smileyMeh.SetActive(true);
            smileyAngry.SetActive(false);
        }

    }


}
