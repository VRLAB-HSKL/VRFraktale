using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sierpinski : MonoBehaviour
{

    public ComputeShader shader;
    ComputeBuffer buffer;
    RenderTexture renderTexture;

    public int increment = 3;
    public float zoomSpeed = 0.5f;
    public int renderWidth = 4096, renderHeight = 4096;

    // Sierpinski parameters
    double width = 5 * 100;
    double height = 2.5 * 100;
    double rStart = -2.85;
    double iStart = -2.55;
    int maxIteration = 1024;

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

        renderTexture = new RenderTexture(renderWidth, renderHeight, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        GetComponent<Renderer>().material.mainTexture = renderTexture;

        UpdateTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTexture()
    {
        int kernelHandle = shader.FindKernel("CSMain");

        buffer.SetData(data);
        shader.SetBuffer(kernelHandle, "buffer", buffer);

        shader.SetInt("iterations", maxIteration);
        shader.SetTexture(kernelHandle, "Result", renderTexture);

        shader.Dispatch(kernelHandle, renderWidth / 24, renderHeight / 24, 1);

        RenderTexture.active = renderTexture;

        /*
        real.text = rStart.ToString();
        imag.text = iStart.ToString();
        w.text = width.ToString();
        h.text = height.ToString();
        ite.text = maxIteration.ToString();

        frame.text = Time.deltaTime.ToString();
        */
    }


    void OnDestroy()
    {
        buffer.Dispose();
    }

    public void NewZoomIn()
    {
        maxIteration = Mathf.Max(100, maxIteration + increment);

        double wFactor = -(width * zoomSpeed * 0.1f);
        double hFactor = -(height * zoomSpeed * 0.1f);
        width -= wFactor;
        height -= hFactor;
        rStart += wFactor / 2.0;
        iStart += hFactor / 2.0;

        data[0].w = width;
        data[0].h = height;
        data[0].r = rStart;
        data[0].i = iStart;

        UpdateTexture();

        
    }
    public void NewZoomOut()
    {
        maxIteration = Mathf.Max(100, maxIteration + increment);

        double wFactor = -(width * zoomSpeed * 0.1f);
        double hFactor = -(height * zoomSpeed * 0.1f);
        width += wFactor;
        height += hFactor;
        rStart -= wFactor / 2.0;
        iStart -= hFactor / 2.0;

        data[0].w = width;
        data[0].h = height;
        data[0].r = rStart;
        data[0].i = iStart;

        UpdateTexture();


    }

    public void NewUp()
    {
        
        iStart -= height * zoomSpeed * 0.1f;

        data[0].i = iStart;

        UpdateTexture();
    }
    public void NewDown()
    {

        iStart += height * zoomSpeed * 0.1f;

        data[0].i = iStart;

        UpdateTexture();
    }
    public void NewLeft()
    {

        rStart += width * zoomSpeed * 0.1f;

        data[0].r = rStart;

        UpdateTexture();
    }
    public void NewRight()
    {

        rStart -= width * zoomSpeed * 0.1f;

        data[0].r = rStart;

        UpdateTexture();
    }

    public void OnChangeSlider(float newZoomSpeed)
    {
        zoomSpeed = newZoomSpeed;
    }
}
