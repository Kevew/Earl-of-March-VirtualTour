using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


//Basically controlls almost all of the UI elements.
public class UIController : MonoBehaviour
{
    public GameObject enabler;
    public InputField input;
    public Text changer;
    int guideortele;
    GuideSystem guideSystem;
    public Scrollbar scroll;
    public Scrollbar scrollzoom;
    public Scrollbar scrollcamspeed;
    public Scrollbar scrollcamdepth;
    public Camera cam;

    public Toggle zoomInEnable;

    public TouchController touch;

    public AudioSource source;
    public Scrollbar audioscroll;

    public GameObject options;
    public GameObject optiontext;
    public float minDepth;
    public GameObject tutorial;
    public GameObject controls;

    public GameObject tutorialMenu;

    public GameObject locationReached;

    public GameObject StartScreen;

    //Guide System Objects
    public GameObject movementButton;
    public GameObject teleButton;

    public GameObject sphereMesh;

    public GameObject enableGuideSystemUIbutton;

    //Teacher UI Info
    public GameObject closeTeacherinfo;

    public TextMeshProUGUI toprightcorner;

    public Dictionary<string, int> info = new Dictionary<string, int>();
    public Dictionary<int, string> seinfo = new Dictionary<int, string>();

    //Calls the setup function + assigsns some variables
    void Start()
    {
        guideortele = 1;
        setup();
        guideSystem = GetComponent<GuideSystem>();
    }

    //Changes the top right corner UI
    public void checkTopRight(string _change)
    {
        toprightcorner.text = _change;
    }

    public void openControls()
    {
        controls.SetActive(true);
        tutorial.SetActive(false);
        optiontext.SetActive(false);
        tutorialMenu.SetActive(false);
    }

    //Opens up the guidesystem UI
    public void enableGuideSystemUIButton()
    {
        if (enableGuideSystemUIbutton.activeSelf)
        {
            enableGuideSystemUIbutton.SetActive(false);
        }
        else
        {
            enableGuideSystemUIbutton.SetActive(true);
        }
    }

    bool fun = false;

    //Change color of guidesystem UI area
    private void FixedUpdate()
    {
        if(guideortele == 1)
        {
            movementButton.GetComponent<Image>().color = Color.green;
            teleButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            movementButton.GetComponent<Image>().color = Color.red;
            teleButton.GetComponent<Image>().color = Color.green;
        }
        
    }

    //This disable the startscreen which is the first UI element you see
    public void disableStartScreen()
    {
        StartScreen.SetActive(false);
    }

    //Disable the locationreached or the UI element you see when you reached the end of your path
    public void disableLocationReached()
    {
        locationReached.SetActive(false);
    }

    //Closes the settings ui and opens the tutorial UI
    public void entertutorial()
    {
        optiontext.SetActive(false);
        tutorial.SetActive(true);
        tutorialMenu.SetActive(true);
        controls.SetActive(false);
    }
    //Closes tutorial Ui and opens settings
    public void enterSettings()
    {
        optiontext.SetActive(true);
        tutorial.SetActive(false);
        tutorialMenu.SetActive(false);
        controls.SetActive(false);
    }

    //Opens the option menu + little animation
    public void openOptions()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (tutorial.activeSelf){
            tutorial.SetActive(false);
            options.SetActive(false);
            return;
        }
        controls.SetActive(false);
        //Disables the welcome to Earl screen so that it doesn't collide with the settings UI
        if (StartScreen.activeSelf)
        {
            StartScreen.SetActive(false);
        }
        Animator temp = options.GetComponent<Animator>();
        options.SetActive(true);
        optiontext.SetActive(true);
        temp.SetBool("OptionOpen",true);
    }
    //The reason I have a seperate function for this is because you need the function to be IEnumerator 
    //To have WaitForSeconds. This closes the options menu
    public void closeOptions()
    {
        StartCoroutine(removeoptionsmenu());
    }

    IEnumerator removeoptionsmenu()
    {
        Animator temp = options.GetComponent<Animator>();
        controls.SetActive(false);
        tutorial.SetActive(false);
        optiontext.SetActive(false);
        tutorialMenu.SetActive(false);
        temp.SetBool("OptionOpen", false);
        yield return new WaitForSeconds(0.5f);
        if (touch.firstperson.isOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        options.SetActive(false);
    }
    
    //Constantly sets the camera field of view to what you have set in the settings. It doesn't do it when the 
    //zoom in affect is working though.
    //Also sets the volume according to what is in the settings
    void Update()
    {
        if(Movement.test == false){
            cam.fieldOfView = Mathf.Max(scroll.value * 100, minDepth);
        }
        source.volume = audioscroll.value;
    }

    //This basically gets all the information from the assets part of the unity.
    //Basically the scripts are not connected with the assets and this function of code does it
    void setup()
    {
        Object path = Resources.Load("GuideChangeInformation");
        TextAsset reader = path as TextAsset;
        string temp = reader.text;
        string[] lines = temp.Split("\n"[0]);
        foreach (string line in lines)
        {
            info[line.Substring(0, 4)] = int.Parse(line.Substring(5, 3));
            seinfo[int.Parse(line.Substring(5, 3))] = line.Substring(0, 4);
        }
    }
    //Controls the small little UI button in the bottem left corner of the screen.
    public void onButtonPush()
    {
        if (enabler.activeSelf)
        {
            enabler.SetActive(false);
            changer.text = "+";
        }
        else
        {
            enabler.SetActive(true);
            changer.text = "-";
        }
    }

    //Sets the user to wanting to get a path from the guide system
    public void setGuide(){
        guideortele = 1;
    }
    //Sets the user to wanting to immediately go to a location from the guide system
    public void setTeleport()
    {
        guideortele = 0;
    }

    //This activates the guide system.
    //If you are set to finding a path, it will call the path
    //If you are sest to teleporting, you will immediately reach the location
    //But first it gotta check if the place is a real location, then check whether you are already at the locaiton
    //Then it calls the functions
    public void enterButtonPush(){
        if(info.ContainsKey(input.text)){
            if (guideortele == 1){
                if(sphereMesh.GetComponent<Renderer>().material != guideSystem.a[info[input.text]]){
                    guideSystem.findpath(sphereMesh.GetComponent<Renderer>().material, guideSystem.a[info[input.text]]);
                }else{
                    input.text = "Error:Same location";
                }
            }else{
                if (sphereMesh.GetComponent<Renderer>().material != guideSystem.a[info[input.text]]){
                    guideSystem.teleportsystem(guideSystem.a[info[input.text]]);
                }else{
                    input.text = "Error:Same location";
                }
            }
        }else{
            input.text = "Error:Incorrect input";
        }
    }
}
