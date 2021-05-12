using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//controls animation and fading of the hitscore text, which corresponds to the Misses, Perfects, Goods, etc that appear above the triangle
public class HitScoreText : MonoBehaviour
{

    public float floatSpeed = 5f;
    public float maxRandomSpawnX = 0.5f;
    public float fadeSpeed = 1f;

    public Transform anchor;
    public TextMeshProUGUI displayText;


    void OnDisable() {
        Destroy(this.gameObject);
    }

    public void Init(string text, Transform anchor, Color color, float yOffset)
    {
        this.anchor = anchor;
        this.transform.position = new Vector3(anchor.position.x + Random.Range(-maxRandomSpawnX, maxRandomSpawnX), anchor.position.y + yOffset, anchor.position.z);
        this.transform.SetParent(anchor);

        this.displayText.color = color;
        displayText.text = text;
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        while (displayText.color.a > 0f) {
            displayText.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);
            this.transform.position += new Vector3(0f, floatSpeed * Time.deltaTime, 0f);


            yield return null;
        }
        Destroy(this.gameObject);
    }

        
}
