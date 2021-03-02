using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//cosmetic hold slider used in the editor
public class EditorHoldSlider : MonoBehaviour, EditorSliderInterface
{

    public LevelSliderType levelSliderType;
    public Transform body;
    public SpriteRenderer bodySprite;
    public Transform endSlider;
    public SpriteRenderer startSliderSpriteRenderer;
    public SpriteRenderer endSliderSpriteRenderer;
    public Transform start;
    public bool isStartSlider = false;
    public bool isEndSlider = false;
    public Transform root;
    public float length = 5f;
    public Color selectedColor;
    public EditorHoldSlider rootScript;
    Color originalColor;

    void Start() {
        if(isStartSlider || isEndSlider)
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void Initialize(float length)
    {
        this.length = length;
        endSlider.position = new Vector3(start.transform.position.x + this.length, endSlider.position.y, endSlider.position.z);
        body.localScale = new Vector3((endSlider.localPosition.x / 10f), body.localScale.y, 0f);
    }

    public void OnDragDelegate()
    {
        if (isStartSlider)
            root.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, root.position.y, root.position.z);
        else if (isEndSlider)
        {
            float newLength = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - start.position.x;
            if (newLength <= 0.0f) newLength = 0.01f;
            rootScript.length = newLength;
            length = newLength;
            Initialize(length);
        }
    }

    public void Select()
    {  
        if(isStartSlider) 
            startSliderSpriteRenderer.color = selectedColor;
        else if(isEndSlider)
            endSliderSpriteRenderer.color = selectedColor;
    }

    public void Unselect()
    {
        if (isStartSlider)
            startSliderSpriteRenderer.color = originalColor;
        else if (isEndSlider)
            endSliderSpriteRenderer.color = originalColor;
    }

    public void Destroy()
    {
        Destroy(root.gameObject);
    }

}
