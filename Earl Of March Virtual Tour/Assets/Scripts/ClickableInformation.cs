using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickableInformation : MonoBehaviour
{
    public Camera cam;
    public Animator popupAnim;
    public GameObject textEnable;

    public GameObject popupGO;

    List<GameObject> currPopups = new List<GameObject>();

    private Dictionary<int, List<node>> listofPops = new Dictionary<int, List<node>>();
    void Awake()
    {
        for (int j = 0; j <= 200; j++)
        {
            listofPops[j] = new List<node>();
        }
        popupAnim.gameObject.SetActive(false);
        setup();
        textEnable.SetActive(false);
    }

    public class node
    {
        public Vector3 location;
        public Vector3 scale;
        public string title;
        public string info;
        public node(Vector3 _location, Vector3 _scale, string _title, string _info)
        {
            location = _location;
            scale = _scale;
            title = _title;
            info = _info;
        }
    }


    //Using InformationBanner.txt, the code gets all the values for where each location should have popups
    private void setup()
    {
        Object path = Resources.Load("InformationBanner");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        int i = 0;
        int place = 0;
        //Vector3.up is placeholder
        Vector3 location = Vector3.up,scale = Vector3.up;
        string titletext = "";
        foreach (string line in lines)
        {
            if(i%3 == 0)
            {
                place = int.Parse(line.Substring(0, 3));
                location = new Vector3(float.Parse(line.Substring(4,5)),
                                       float.Parse(line.Substring(10,5)),
                                       float.Parse(line.Substring(16,5)));
                scale = new Vector3(float.Parse(line.Substring(22, 5)),
                                       float.Parse(line.Substring(28, 5)),
                                       float.Parse(line.Substring(34, 5)));

            }
            else if(i%2 == 1)
            {
                titletext = line;
            }
            else
            {
                listofPops[place].Add(new node(location, scale, titletext, line));
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




    public void LoadNewMovements(int currentLoc)
    {
        if (listofPops[currentLoc].Count > 0)
        {
            Debug.Log("TEST");
            foreach (node g in listofPops[currentLoc])
            {
                GameObject a = (GameObject)Instantiate(popupGO, g.location, transform.rotation);
                a.transform.localScale = g.scale;
                a.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = g.title;
                a.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = g.info;
                currPopups.Add(a);
            }
        }
    }
    public void deleteCurrentMovements()
    {
        foreach (GameObject g in currPopups)
        {
            Destroy(g);
        }
        currPopups = new List<GameObject>();
    }
}
