using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CSMandelbrot : MonoBehaviour
{
    //Shader resources
    public ComputeShader shader;
    ComputeBuffer buffer;
    RenderTexture renderTexture;

    //GUI Resources
    public int increment = 0; //Change to get more Iterations while zooming in
    public float zoomSpeed = 0.05f;
    public int renderWidth = 4096, renderHeight = 4096;


    //Mandelbrot parameters
    double width = 5;
    double height = 2.5;
    double rStart = -2.85;
    double iStart = -2.55;
    int maxIteration = 512;


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

    private void Update()
    {
       /* if(Input.GetMouseButton(0))
        {
            StartCoroutine(ZoomingIn());
        }

        if (Input.GetMouseButton(1))
        {
            StartCoroutine(ZoomingOut());
        }

        if (Input.GetMouseButtonDown(2))
        {
            CenterScreen();
        }
       */
    }

    IEnumerator ZoomingIn()
    {
        yield return StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomingOut()
    {
        yield return StartCoroutine(ZoomOut());
    }


    IEnumerator ZoomIn()
    {
        maxIteration = Mathf.Max(100, maxIteration + increment);
        

        double wFactor = width * zoomSpeed * Time.deltaTime;
        double hFactor = height * zoomSpeed * Time.deltaTime;
        width -= wFactor;
        height -= hFactor;
        rStart += wFactor / 2.0;
        iStart += hFactor / 2.0;

        data[0].w = width;
        data[0].h = height;
        data[0].r = rStart;
        data[0].i = iStart;

        UpdateTexture();

        yield return null;
    }

    IEnumerator ZoomOut()
    {
        maxIteration = Mathf.Max(100, maxIteration - increment);
        

        double wFactor = width * zoomSpeed * Time.deltaTime;
        double hFactor = height * zoomSpeed * Time.deltaTime;
        width += wFactor;
        height += hFactor;
        rStart -= wFactor / 2.0;
        iStart -= hFactor / 2.0;

        data[0].w = width;
        data[0].h = height;
        data[0].r = rStart;
        data[0].i = iStart;

        UpdateTexture();

        yield return null;
    }

    void CenterScreen()
    {
        rStart += (Input.mousePosition.x - (renderWidth / 2.0)) / renderWidth * width;
        iStart += (Input.mousePosition.y - (renderHeight / 2.0)) / renderHeight * height;

        data[0].r = rStart;
        data[0].i = iStart;

        UpdateTexture();
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
    }


    void OnDestroy()
    {
        buffer.Dispose();
    }

    public void NewZoomIn()
    {
        maxIteration = Mathf.Max(100, maxIteration + increment);

        double wFactor = width * zoomSpeed * 0.1f;
        double hFactor = height * zoomSpeed * 0.1f;
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

        double wFactor = width * zoomSpeed * 0.1f;
        double hFactor = height * zoomSpeed * 0.1f;
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
