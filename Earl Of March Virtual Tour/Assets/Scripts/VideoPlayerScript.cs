using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public Animator anim;
    void Awake(){
        StartCoroutine(StartAnim());
    }


    IEnumerator StartAnim(){
        yield return new WaitForSeconds(1f);
        anim.SetBool("StartAnim", true);
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("StartAnim", false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
