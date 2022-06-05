using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTowardsCamera : MonoBehaviour
{
    public Transform cam;
    private Image change;
    public float displayInfo;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        change = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }

    public void changeInfo()
    {
        displayInfo += Time.deltaTime*4;
        displayInfo = Mathf.Min(displayInfo, 2f);
    }

    public float currentDisplayInfo()
    {
        return displayInfo;
    }

    public string currentNameInfo()
    {
        return transform.name;
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2*transform.position - cam.position);
        change.fillAmount = Mathf.Min(displayInfo, 1f);
        if(displayInfo > 0f)
        {
            displayInfo -= Time.deltaTime * 2;
            if(displayInfo < 0f)
            {
                displayInfo = 0f;
            }
        }
    }
}
