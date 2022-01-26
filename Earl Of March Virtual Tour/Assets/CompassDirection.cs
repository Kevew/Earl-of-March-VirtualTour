using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassDirection : MonoBehaviour
{
    public Transform camera;
    public RawImage compassimage;

    public float change;
    //Code to manipulate the movement of the compass
    void Update()
    {
        compassimage.uvRect = new Rect(camera.localEulerAngles.y / change, 0f, 1f, 1f);
    }
}
