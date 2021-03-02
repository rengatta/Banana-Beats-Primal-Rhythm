using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gradient : MonoBehaviour
{
    public UnityEngine.UI.RawImage img;

    private Texture2D backgroundTexture;

    public Color topColor = Color.white;
    public Color bottomColor = Color.black;

    void OnValidate()
    {
        backgroundTexture = new Texture2D(1, 2);
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
        SetColor(bottomColor, topColor);
    }


    public void SetColor(Color color1, Color color2)
    {
        backgroundTexture.SetPixels(new Color[] { color1, color2 });
        backgroundTexture.Apply();
        img.texture = backgroundTexture;
    }
}
