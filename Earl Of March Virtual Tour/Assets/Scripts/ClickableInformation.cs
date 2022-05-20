using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickableInformation : MonoBehaviour
{
    public Camera cam;
    public Animator popupAnim;
    public GameObject textEnable;

    void Awake()
    {
        popupAnim.gameObject.SetActive(false);
        textEnable.SetActive(false);
    }

    void Update()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject != null)
            {
                if(hitInfo.collider.tag == "PopUp")
                {
                    hitInfo.transform.GetComponent<FaceTowardsCamera>().changeInfo();
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayForHit = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayForHit, out RaycastHit hit))
            {
                if (hit.collider.gameObject != null)
                {
                    if (hit.collider.tag == "PopUp")
                    {
                        float curr = hit.transform.GetComponent<FaceTowardsCamera>().currentDisplayInfo();
                        if(curr >= 1f)
                        {
                            intializefunction();
                        }
                    }
                }
            }
        }
    }


    void intializefunction()
    {
        popupAnim.gameObject.SetActive(true);
        popupAnim.SetBool("Enabled", true);
        StartCoroutine(enableText());
    }

    IEnumerator enableText()
    {
        yield return new WaitForSeconds(0.3f);
        textEnable.SetActive(true);
    }


    public void closePopup()
    {
        textEnable.SetActive(false);
        popupAnim.SetBool("Enabled", false);
        StartCoroutine(actualclosePopUp());
    }

    IEnumerator actualclosePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        popupAnim.gameObject.SetActive(false);
    }



}
