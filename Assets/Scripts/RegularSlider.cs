using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SliderType
{
    LeftSlider,
    RightSlider
}

//interface shared between RegularSliders and HoldSliders
public interface SliderInterface
{
    SliderType sliderType { get; set; }

    float horizontal_speed { get; set; }

    bool can_destroy { get; set; }

    void Darken();

    void DetectHit();

    float score { get; set; }

    bool hit { get; set; }
}


public class RegularSlider : MonoBehaviour, SliderInterface
{

    [HideInInspector]
    public bool can_destroy { get; set; } = true;
    public float horizontal_speed { get; set; }
    public SliderType sliderType { get { return _sliderType; } set { _sliderType = value; } }
    [SerializeField]
    private SliderType _sliderType;
    public Vector3 direction = Vector3.right;
    public SpriteRenderer spriteRenderer;
    public KeyCode clickKey;

    [HideInInspector]
    public float score { get; set; } = 10f;
    HitScore hitScore;

    public bool hit { get; set; } = false;
    public GameObject hitScoreTextPrefab;

    public void Darken() {
        Color tempColor = Color.black;
        tempColor.a = 0.4f;
        spriteRenderer.color = tempColor;
    }


     float glowDuration = 0.05f;
     float fadeSpeed = 10f;
   
    IEnumerator GlowFadeOut() {

        spriteRenderer.material.SetFloat("_Intensity", 4.5f);

      
         //gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_YourParameter", someValue);
        yield return new WaitForSeconds(glowDuration);

        while(spriteRenderer.color.a > 0f) {
            spriteRenderer.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        
       
        Destroy(this.gameObject);
    }


    void HitDetected() {
        hit = true;
        StartCoroutine(GlowFadeOut());
    }

    public void DetectHit() {
        if (Input.GetKeyDown(clickKey) && !GameState.paused && !hit)
        {

            
            if (hitScore == HitScore.Perfect)
            {
                GlobalHelper.global.scoreManager.totalHits += 1;
                GlobalHelper.global.scoreManager.score += SceneToSceneData.sliderScore* SceneToSceneData.perfectMultiplier;

                GlobalHelper.global.scoreManager.combo += 1;

                GlobalHelper.global.SpawnPerfectText();

                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Happy);
            }
            else if (hitScore == HitScore.Good)
            {
                GlobalHelper.global.scoreManager.totalHits += 1;
                GlobalHelper.global.scoreManager.score += SceneToSceneData.sliderScore;
                GlobalHelper.global.scoreManager.combo += 1;

                GlobalHelper.global.SpawnGoodText();
  
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Happy);
            }

            HitDetected();
        }

    }

    void Start() {
        StartCoroutine(UpdateWithSongTime());
        spriteRenderer.color += new Color(0.05f, 0.05f, 0.05f);
    }


    IEnumerator UpdateWithSongTime() {

        while (true) {
            yield return new WaitForEndOfFrame();
            this.transform.position += (direction * horizontal_speed * (float)GameState.songTimeDelta);

        }

    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == GlobalHelper.perfectRegionLayer)
        {
            hitScore = HitScore.Perfect;

        }
        else if (collision.gameObject.layer == GlobalHelper.goodRegionLayer)
        {

            hitScore = HitScore.Good;

        }
    }

}
