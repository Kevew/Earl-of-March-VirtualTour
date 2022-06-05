using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiControllerTeacherInfo : MonoBehaviour
{

    public GameObject option1;
    public GameObject option2;
    public Transform content;

    public GameObject teacherInfo;

    public GameObject topbar1;
    public GameObject topbar2;
    public GameObject topbar3;

    public TMP_InputField input;

    public TouchController touch;

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

    float alphaofCopyInfoUI;
    private float textSize;
    void Start()
    {
        alphaofCopyInfoUI = 1f;
        setupTeacherinfo();
        getTextSize();
    }

    void getTextSize()
    {
        GameObject b = (GameObject)Instantiate(option1, option1.transform.position, option1.transform.rotation);
        b.transform.SetParent(content.transform, false);
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bonnell, Cynthia Educational Assistant cynthia.bonnell @ocdsb.ca";
        option1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enableAutoSizing = true;
        textSize = option1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize;
        option1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enableAutoSizing = false;
        Destroy(b);
    }
    public void CopyInfo()
    {
        alphaofCopyInfoUI = 3f;
    }

    public Image copyUI;
    public TextMeshProUGUI copyUIText;
    void Update()
    {
        if(alphaofCopyInfoUI < 0f)
        {
            alphaofCopyInfoUI = 0f;
        }
        else
        {
            alphaofCopyInfoUI -= Time.deltaTime;
        }
        if(copyUI.color.a != alphaofCopyInfoUI)
        {
            copyUI.color = new Color(copyUI.color.r, copyUI.color.g, copyUI.color.b, Mathf.Min(1f, alphaofCopyInfoUI));
            copyUIText.color = new Color(copyUIText.color.r, copyUIText.color.g, copyUIText.color.b, Mathf.Min(1f, alphaofCopyInfoUI));
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && teacherInfo.activeSelf)
        {
            searchTeach();
        }
    }

    void setupTeacherinfo()
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

            if (i % 3 == 2)
            {
                if (position == "Secondary\r")
                {
                    secondary.Add(new teacher(name, position, email));
                }
                else if (position == "Intermediate\r")
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
        Animator temp = teacherInfo.GetComponent<Animator>();
        teacherInfo.SetActive(true);
        temp.SetBool("OpenTeacher", true);
        showallstaff();
    }

    public void closeTeacherInfo()
    {
        resetList();
        StartCoroutine(removeTeacherMenu());
    }

    IEnumerator removeTeacherMenu()
    {
        Animator temp = teacherInfo.GetComponent<Animator>();
        temp.SetBool("OpenTeacher", false);
        yield return new WaitForSeconds(1f);
        if (touch.firstperson.isOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        teacherInfo.SetActive(false);
    }
    private void resetList()
    {
        foreach (GameObject a in current)
        {
            Destroy(a);
        }
    }


    private void addNewGameObject(GameObject op, teacher a)
    {
        GameObject b = (GameObject)Instantiate(op, option1.transform.position, option1.transform.rotation);
        b.transform.SetParent(content.transform, false);
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
        b.SetActive(true);
        current.Add(b);
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
                addNewGameObject(option1, a);
            }
            else
            {
                addNewGameObject(option2, a);
            }
            i = 1 - i;
        }
        foreach (teacher a in secondary)
        {
            if(i == 0)
            {
                addNewGameObject(option1, a);
            }
            else
            {
                addNewGameObject(option2, a);
            }
            i = 1 - i;
        }
        foreach (teacher a in intermediate)
        {
            if (i == 0)
            {
                addNewGameObject(option1, a);
            }
            else
            {
                addNewGameObject(option2, a);
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
                addNewGameObject(option1, a);
            }
            else
            {
                addNewGameObject(option2, a);
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
                addNewGameObject(option1, a);
            }
            else
            {
                addNewGameObject(option2, a);
            }
            i = 1 - i;
        }
    }

    public void searchTeach()
    {
        resetList();
        string curr = input.text;
        int i = 0;
        if (topbar3.activeSelf || topbar1.activeSelf)
        {
            foreach (teacher a in secondary)
            {
                string used = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                if (!used.Contains(curr)){
                    continue;
                }
                if (i == 0)
                {
                    GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                else
                {
                    GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                i = 1 - i;
            }
        }
        if (topbar2.activeSelf || topbar1.activeSelf)
        {
            foreach (teacher a in intermediate)
            {
                string used = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                if (!used.Contains(curr))
                {
                    continue;
                }
                if (i == 0)
                {
                    GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                else
                {
                    GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                i = 1 - i;
            }
        }
        if (topbar1.activeSelf)
        {
            foreach (teacher a in others)
            {
                string used = (a.name + ", " + a.position + ", " + a.email).Replace("\r", "").Replace("\n", "");
                if (!used.Contains(curr))
                {
                    continue;
                }
                if (i == 0)
                {
                    GameObject b = (GameObject)Instantiate(option1, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                else
                {
                    GameObject b = (GameObject)Instantiate(option2, transform.position, transform.rotation);
                    b.transform.SetParent(content.transform, false);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = used;
                    b.SetActive(true);
                    current.Add(b);
                }
                i = 1 - i;
            }
        }
    }
}
