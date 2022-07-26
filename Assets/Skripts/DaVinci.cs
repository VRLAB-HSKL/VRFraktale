using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaVinci : MonoBehaviour
{

    private Texture2D drawingTexture;

    private int count = 0;

    [Header("Settings")]
    public int refreshRate = 30;
    [Range(0.01f, 5f)]
    public float scalingFactor = 2;

    void Start()
    {
        // Create 16/9 Texture for the canvas and apply it to the renderer
        this.drawingTexture = new Texture2D(2660, 1440);
        GetComponent<Renderer>().material.mainTexture = drawingTexture;

        DrawSierpinskiTri();

    }

    void Update()
    {
        count ++;

        if (count >= 30)
        {
            DrawSierpinskiTri();
            count = 0;
        }       
    }

    private void DrawSierpinskiTri() 
    {
        for (int y = 0; y < drawingTexture.height; y++)
        {
            for (int x = 0; x < drawingTexture.width; x++)
            {

                Color color = (((int) (x * scalingFactor) & (int) (y * scalingFactor)) != 0 ? Color.white : Color.gray);
                drawingTexture.SetPixel(x, y, color);
            }
        }
        drawingTexture.Apply();
    }
}
