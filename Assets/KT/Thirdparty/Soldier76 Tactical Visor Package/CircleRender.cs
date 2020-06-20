using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRender : MonoBehaviour
{
    public Material mLineMaterial;

    public Camera mCamera;
    private float activeRadius = 0.10f;

    void Awake()
    {
    }

    void Start()
    {
        // Empty
    }

    void OnRenderObject()
    {
        int lineCount = 12;

        List<Vector2> edgeVertices = new List<Vector2>();

        // Apply the line material
        mLineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.LoadOrtho();

        // Draw lines
        GL.Begin(GL.LINES);
        for (int i = 0; i < lineCount; ++i)
        {
            float a = i / (float)lineCount;
            float angle = a * Mathf.PI * 2;

            float xValue = 0.5f + Mathf.Cos(angle) * activeRadius;

            float difference = (float)mCamera.pixelWidth / (float)mCamera.pixelHeight;
            float yValue = 0.5f + (Mathf.Sin(angle) * activeRadius) * difference;

            Vector2 edgeVertex = new Vector2(xValue, yValue);
            edgeVertices.Add(edgeVertex);
        }

        // Whole thing
        for (int i = 1; i < lineCount; i++)
        {
            GL.Vertex(edgeVertices[i - 1]);
            GL.Vertex(edgeVertices[i]);
        }

        // Connect
        GL.Vertex(edgeVertices[0]);
        GL.Vertex(edgeVertices[edgeVertices.Count - 1]);

        GL.End();

        GL.PopMatrix();
    }

}
