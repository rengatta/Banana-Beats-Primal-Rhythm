using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private float _score;
    public float score {
        get {
            return _score;
        }

        set {
            _score = value;

            scoreText.text = "Score: " + ((int)_score).ToString();

        }
    }
    
    private int _combo;
    public int combo { 
    
        get
        {
            return _combo;
        }

        set
        {
            _combo = value;
            comboText.text = "Combo: " + _combo.ToString();
        }
    }


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;



}
