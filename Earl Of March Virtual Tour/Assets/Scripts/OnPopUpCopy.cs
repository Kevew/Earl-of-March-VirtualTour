using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OnPopUpCopy : MonoBehaviour
{

    public UiControllerTeacherInfo ui;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Unique").GetComponent<UiControllerTeacherInfo>();
    }
    public void copyToClip()
    {
        ui.CopyInfo();
        GUIUtility.systemCopyBuffer = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
