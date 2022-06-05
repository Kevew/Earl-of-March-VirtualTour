using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickableInformation : MonoBehaviour
{
    public Camera cam;
    public Animator popupAnim;
    public GameObject textEnable;
    public GameObject titleTextEnable;
    public GameObject closeTextEnable;

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InfoText;

    void Awake()
    {
        popupAnim.gameObject.SetActive(false);
        textEnable.SetActive(false);
        readInformationBanner();
    }


    public class clicknode
    {
        public string title;
        public string text;
        public clicknode(string _title, string _text)
        {
            title = _title;
            text = _text;
        }
    }

    private List<clicknode> listofPopsInfo = new List<clicknode>();

    //Using InformationBanner.txt, the code gets all the values for where each location should have popups
    private void readInformationBanner()
    {
        Object path = Resources.Load("InformationBanner");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int i = 0;
        //Vector3.up is placeholder
        string titleInfo = "";
        foreach (string line in lines)
        {
            if (i % 3 == 1)
            {
                titleInfo = line;
            }
            else if(i%3 == 2)
            {
                listofPopsInfo.Add(new clicknode(titleInfo, line));
            }
            i++;
        }
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
                    if (hit.collider.tag == "PopUp" && !popupAnim.gameObject.activeSelf)
                    {
                        float curr = hit.transform.GetComponent<FaceTowardsCamera>().currentDisplayInfo();
                        string nameHit = hit.transform.GetComponent<FaceTowardsCamera>().currentNameInfo();
                        if (curr >= 1f)
                        {
                            intializefunction();
                            setText(int.Parse(nameHit));
                        }
                    }
                }
            }
        }
    }

    void setText(int curr)
    {
        TitleText.text = listofPopsInfo[curr].title;
        InfoText.text = listofPopsInfo[curr].text;
    }


    void intializefunction()
    {
        popupAnim.gameObject.SetActive(true);
        popupAnim.SetBool("Enabled", true);
        StartCoroutine(enableText());
    }

    IEnumerator enableText()
    {
        yield return new WaitForSeconds(1f);
        textEnable.SetActive(true);
        titleTextEnable.SetActive(true);
        closeTextEnable.SetActive(true);
    }


    public void closePopup()
    {
        textEnable.SetActive(false);
        closeTextEnable.SetActive(false);
        popupAnim.SetBool("Enabled", false);
        titleTextEnable.SetActive(false);
        StartCoroutine(actualclosePopUp());
    }

    IEnumerator actualclosePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        popupAnim.gameObject.SetActive(false);
    }



}
