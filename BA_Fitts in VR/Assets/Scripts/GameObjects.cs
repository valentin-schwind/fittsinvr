using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class GameObjects : MonoBehaviour
{
    public GameObject HandController;

    public GameObject ButtonsContainer;
    public GameObject Buttons;
    public GameObject Confirm;
    [Header("Panel")] public GameObject Panel;
    public GameObject MyTable;
    public GameObject TableTop;
    public GameObject Fragebogen;
    public GameObject PreFrage;

    [Header("ManusVR")] public GameObject ManusVR;
    public GameObject Right;
    public GameObject Left;


    public GameObject IndexCollider;
    public GameObject R_Palm;

    public void OnApplicationQuit()
    {
        SaveClickData.closeIfOpen();
        SaveQuestionnaire.CloseIfOpen();
        SaveHandPosition.closeIfOpen();
    }
}