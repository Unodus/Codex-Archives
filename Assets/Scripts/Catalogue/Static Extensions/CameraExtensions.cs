using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions 
{
    // Camera extension to get its orthographic bounds, use the depth to have thicker bounds, used to see if something is visible in an orthragraphic camera
    public static Bounds OrthographicBounds(this Camera camera, float aspectRatio, float depth = 0)
    {
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * aspectRatio, cameraHeight, depth));

        return bounds;
    }
}
