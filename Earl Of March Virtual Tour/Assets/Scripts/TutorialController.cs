using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> tutoriallist;

    int currentactivate = 0;


    //Tutorial UI Controller
    //Basically it controls which text shows up when you click the information page int the tutorial page.
    //It first disables the current one.
    //Enable the new one based on what we just clicked
    //and then set the current one equal to the one we clicked so when we click another one, this one get's disabled
    public void activate()
    {
        tutoriallist[currentactivate].SetActive(false);
        tutoriallist[int.Parse(EventSystem.current.currentSelectedGameObject.name)].SetActive(true);
        currentactivate = int.Parse(EventSystem.current.currentSelectedGameObject.name);
    }
}
