using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenQuad : MonoBehaviour
{
    [SerializeField]
    private Material mat;
    void OnRenderObject()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Vertex3(-1, -1, 0);
        GL.Vertex3(-1, 1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(1, -1, 0);
        GL.End();
        GL.PopMatrix();
    }
}
