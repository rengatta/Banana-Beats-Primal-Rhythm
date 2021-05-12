using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Utilities;
//used to store the scores and populate the relevant UIs when something is changed
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
            if (_combo > highestCombo)
            {
                highestCombo = _combo;
            }

            comboText.text = "Combo: " + _combo.ToString();
        }
    }
    [HideInInspector]
    public int totalHits = 0;
    public int highestCombo = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public AudioSource audioSource;
    public bool songStarted = false;
    public SceneField levelCompleteScene;

  



}
