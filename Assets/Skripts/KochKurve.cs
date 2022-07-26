using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KochKurve : MonoBehaviour
{

    public ComputeShader shader;
    ComputeBuffer buffer;
    RenderTexture renderTexture;

    public int increment = 3;
    public float zoomed = 0;
    public int renderWidth = 4096, renderHeight = 4096;

    // Kochkurve parameters
    double width = 5;
    double height = 2.5;
    double rStart = -2.85;
    double iStart = 10;
    int maxIteration = 20;
    float size = 0.5f;
    float moveX = 800;
    float moveY = 800;

    //GUI
    public TextMeshProUGUI textMeshIteration;

    int iterations = 16;
    int iterationIncrements = 2;
    bool swaped = false;

    //Data for the Compute Shader
    public struct DataStruct
    {
        public double w, h, r, i;
        public int screenWidth, screenHeight;
    }

    DataStruct[] data = new DataStruct[1];

    // Start is called before the first frame update
    void Start()
    {
        height = width * renderHeight / renderWidth;

        data[0] = new DataStruct
        {
            w = width,
            h = height,
            r = rStart,
            i = iStart,
            screenWidth = renderWidth,
            screenHeight = renderHeight
        };

        buffer = new ComputeBuffer(1, 40);
        textMeshIteration.text = "Iteration: " + iterations;

        renderTexture = new RenderTexture(renderWidth, renderHeight, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        GetComponent<Renderer>().material.mainTexture = renderTexture;

        UpdateTexture();
    }

    void UpdateTexture()
    {
        int kernelHandle = shader.FindKernel("CSMain");

        buffer.SetData(data);
        shader.SetBuffer(kernelHandle, "buffer", buffer);

        shader.SetTexture(kernelHandle, "Result", renderTexture);
        shader.SetFloat("size", size);
        shader.SetFloat("moveX", moveX);
        shader.SetFloat("moveY", moveY);
        shader.Dispatch(kernelHandle, renderWidth / 24, renderHeight / 24, 1);


        RenderTexture.active = renderTexture;

        
    }


    void OnDestroy()
    {
        buffer.Dispose();
    }

    public void NewUp()
    {
        if (zoomed != 0)
            moveY -= size * 0.1f * moveY;

        UpdateTexture();
    }
    public void NewDown()
    {
        if (zoomed != 0)
        moveY += size * 0.1f * moveY;
        UpdateTexture();
    }

    public void NewZoomIn()
    {
        zoomed++;
        size += size * 0.1f + 0.1f;
        
        Debug.Log(size);

        UpdateTexture();

        
    }
    public void NewZoomOut()
    {
        if (zoomed != 0) zoomed--;
        size -= size *0.1f + 0.1f;
        
        if (size < 0.5f)
        {
            size = 0.5f;
            moveY = 800f;
        }
        UpdateTexture();
    }

    public void NewInkrementPlus()
    {
        if(iterations < maxIteration)
        {
            iterations += iterationIncrements;
            textMeshIteration.text = "Iteration: " + iterations;
            data[0].i = iterations;
            UpdateTexture();
        }
        else
        {
            textMeshIteration.text = "Iteration: MAX" ;
        }

    }
    public void NewInkrementMinus()
    {
        if(iterations > 0)
        {
            iterations -= iterationIncrements;


            data[0].i = iterations;
            textMeshIteration.text = "Iteration: " + iterations;

            UpdateTexture();
        }
        else
        {
            textMeshIteration.text = "Iteration: MIN";
        }


    }
    public void NewInvert()
    {
        if (swaped) {
            iterations --;
            swaped = !swaped;
        }
            
        else {
            iterations ++;
            swaped = !swaped;
        }
        data[0].i = iterations;
        textMeshIteration.text = "Iteration: " + iterations;

        UpdateTexture();
    }

}
