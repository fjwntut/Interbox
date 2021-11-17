using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]

public class flipCam : MonoBehaviour
{    new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
    camera = GetComponent<Camera>();
    }
 void OnPreCull()
{
    camera.ResetWorldToCameraMatrix();
    camera.ResetProjectionMatrix();
    Vector3 scale = new Vector3(-1, 1, 1);
    camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
}

void OnPreRender()
{
    GL.invertCulling = true;
}

void OnPostRender()
{
    GL.invertCulling = false;
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
