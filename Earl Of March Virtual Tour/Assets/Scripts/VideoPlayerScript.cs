using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    private VideoPlayer vid;
    public Animator anim;
    void Awake(){
        vid = GetComponent<VideoPlayer>();
    }

    //
    float waits = 0,temp = 5f;
    bool movement = false;
    Vector3 lastMouseCoordinate = Vector3.zero;
    //Video related script + anim for skip button
    void Update()
    {
        //If the video is not playing (the video has ended), then call the leave scene. I have the waits to be bigger than 0.5f
        //seconds as the video takes about some milliseconds to start up. Without it, we will be moved to the main scene
        //WIthout seeing the video
        if (!vid.isPlaying && waits >= 0.5)
        {
            ExitVideo();
        }
        waits += Time.deltaTime;

        //Check if the previous location was my current location
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        //If it is, set temp = 0, basically reseting the timer for the skip animation
        if (mouseDelta != Vector3.zero){
            temp = 0;
        }
        //Set the lastmousecoordinate so that the next frame, it will check the current frame
        lastMouseCoordinate = Input.mousePosition;
        temp += Time.deltaTime;
        //Animation part
        if(temp <= 2f){
            anim.SetBool("MouseMovement", true);
        }else{
            anim.SetBool("MouseMovement", false);
        }
     }

    //When the video ends or you click skip, you can leave the scene to the main one.
    public void ExitVideo()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
