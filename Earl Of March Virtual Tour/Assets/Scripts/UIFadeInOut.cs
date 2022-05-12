using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(startApplication());
    }

    public void callend()
    {
        StartCoroutine(endApplication());
    }

    IEnumerator startApplication()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("Transition", true);
        anim.gameObject.GetComponent<Image>().enabled = false;
    }

    IEnumerator endApplication()
    {
        anim.gameObject.GetComponent<Image>().enabled = true;
        anim.SetBool("Transition", false);
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
