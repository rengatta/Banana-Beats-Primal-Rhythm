using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface for functionality shared between EditorSliders and EditorHoldSliders
public interface EditorSliderInterface
{
    void Select();
    void Unselect();
    void Destroy();
}

//cosmetic slider used in the editor
public class EditorSlider : MonoBehaviour, EditorSliderInterface
{
    public LevelSliderType levelSliderType;
    public SpriteRenderer spriteRenderer;
    public Color selectedColor;

    public void Drag()
    {
        this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, this.transform.position.y, this.transform.position.z);
    }

    public void Select()
    {
        spriteRenderer.color = selectedColor;
    }

    public void Unselect() {
        
        spriteRenderer.color = Color.white;
    }


    public void Destroy()
    {
        Destroy(this.gameObject);
    }


}
