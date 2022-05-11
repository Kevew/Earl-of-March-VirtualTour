using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UiControllerTeacherInfo : MonoBehaviour
{

    public GameObject option1;
    public GameObject option2;
    public Transform content;

    public GameObject teacherInfo;

    public GameObject topbar1;
    public GameObject topbar2;
    public GameObject topbar3;

    public class teacher
    {
        public string name;
        public string position;
        public string email;
        public teacher(string _name, string _position, string _email)
        {
            name = _name;
            position = _position;
            email = _email;
        }
    }

    private List<teacher> secondary = new List<teacher>();
    private List<teacher> intermediate = new List<teacher>();
    private List<teacher> others = new List<teacher>();

    private List<GameObject> current = new List<GameObject>(); 

    public void copyToClip()
    {
        GUIUtility.systemCopyBuffer = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
    void Start()
    {
        Object path = Resources.Load("TeacherInformation");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int i = 0;
        string name = "";
        string position = "";
        foreach (string email in lines)
        {

            if (i%3 == 2)
            {
                if(position == "Secondary\r")
                {
                    secondary.Add(new teacher(name, position, email));
                }
                else if(position == "Intermediate\r")
                {
                    intermediate.Add(new teacher(name, position, email));
                }
                else
                {
                    others.Add(new teacher(name, position, email));
                }
            }
            i++;
            name = position;
            position = email;
        }
    }

    public void openTeacherInfo()
    {
        teacherInfo.SetActive(true);
        showallstaff();
    }

    public void closeTeacherInfo()
    {
        teacherInfo.SetActive(false);
    }

    private void resetList()
    {
        foreach (GameObject a in current)
        {
            Destroy(a);
        }
    }

    public void showallstaff()
    {
        resetList();
        int i = 0;
        topbar1.SetActive(true);
        topbar2.SetActive(false);
        topbar3.SetActive(false);
        foreach (teacher a in others)
        {
            if (i == 0)
            {
                GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            else
            {
                GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            i = 1 - i;
        }
        foreach (teacher a in secondary)
        {
            if(i == 0)
            {
                GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            else
            {
                GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            i = 1 - i;
        }
        foreach (teacher a in intermediate)
        {
            if (i == 0)
            {
                GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            else
            {
                GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            i = 1 - i;
        }
    }


    public void showIntermediateStaff()
    {
        resetList();
        int i = 0;
        topbar1.SetActive(false);
        topbar2.SetActive(true);
        topbar3.SetActive(false);
        foreach (teacher a in intermediate)
        {
            if (i == 0)
            {
                GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            else
            {
                GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            i = 1 - i;
        }
    }

    public void showSecondaryStaff()
    {
        topbar1.SetActive(false);
        topbar2.SetActive(false);
        topbar3.SetActive(true);
        resetList();
        int i = 0;
        foreach (teacher a in secondary)
        {
            if (i == 0)
            {
                GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            else
            {
                GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                b.transform.SetParent(content);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                b.SetActive(true);
                current.Add(b);
            }
            i = 1 - i;
        }
    }

}
