using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public float Speed = 5f;
    private float pitch = 0f;
    private float yaw = 0f;
    KeyCode a;

    private void Start(){
        yaw = 0f;
        pitch = 0f;
        a = KeyCode.F;
    }

    private void Update(){
        if (Input.GetMouseButton(0))
        {
            pitch += Speed * Input.GetAxis("Mouse Y");

            yaw += Speed * -Input.GetAxis("Mouse X");

            pitch = Mathf.Clamp(pitch, -90f, 90f);
            
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }
}
