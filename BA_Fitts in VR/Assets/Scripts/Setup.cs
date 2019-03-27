using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.VR;
using Random = UnityEngine.Random;

//Class for setting up the scene. Creating Buttons etc.

public class Setup : MonoBehaviour
{
    public static Setup SetupInstance;
    public GameObject GameController;
    private GameObjects _objects;
    public Transform Btn; 
    private const int CircleAmount = 15;
    public int AmountOfRounds;
    public int AmountOfTrials;
    private readonly float[] _buttonWidths = {8, 6, 8, 4, 3, 4, 2, 2, 2, 1, 8, 6, 8, 4, 3, 4, 2, 2, 2, 1};
    private readonly float[] _amplitudes = {12, 14, 28, 18, 18, 30, 20, 26, 31, 31.5f, 12, 14, 28, 18, 18, 30, 20, 26, 31, 31.5f};
//    private readonly float[] _buttonWidths = {4,3,8};
//    private readonly float[] _amplitudes = {18,30,12};
    [NonSerialized] public float[] Ids;
    private float Radius = 0.01f;
    public int UserId;

    [Header("Only change this if program crashed or sth")]
    public int CurrentTrial = 0;


    public void StartSetup()
    {
        Variables.Trial = CurrentTrial;
        
        Variables.RoundList = new int[AmountOfRounds];

        int j = 0;
        while (j < Variables.RoundList.Length)
        {
            Variables.RoundList[j] = j;
            j++;
        }
        ShuffleList(Variables.UserId, Variables.Trial);
        if (!(_objects.Buttons.transform.childCount > 0))
        {
            //CreateButtons();
            StartCoroutine(SetupPreQuestionnaire());
        }
        else print("buttons already exist");
        
    }

    public void ShuffleList(int UserID, int trial)
    {
        var r = UserID * 100 + trial;
        Random.InitState(r);
        for (int i = 0; i < Variables.RoundList.Length; i++) {
            int temp = Variables.RoundList[i];
            int randomIndex = Random.Range(i, Variables.RoundList.Length);
            Variables.RoundList[i] = Variables.RoundList[randomIndex];
            Variables.RoundList[randomIndex] = temp;
        }
    }
    
    IEnumerator SetupPreQuestionnaire()
    {
        var s = _objects.Buttons.GetComponent<Scenes>();
        yield return StartCoroutine(s.CreatePreQuestionnaire());

        print("setup");
        CreateButtons();
    }

    private void Start()
    {
        if (_buttonWidths.Length != _amplitudes.Length)
        {
            print("Unequal Buttonwidths and Amplitudes");
        }
        Ids = new float[AmountOfRounds];
        for (int i = 0; i < AmountOfRounds; i++)
        {
            Ids[i] = Mathf.Log((_amplitudes[i] / (_buttonWidths[i])) + 0.5f, 2);
        }

        _objects = GameController.GetComponent<GameObjects>();
    }

    private void Awake()
    {
        Variables.UserId = UserId;
        Variables.AmountOfRounds = AmountOfRounds;
        Variables.AmountOfTrials = AmountOfTrials;
        SetupInstance = this;
    }

    public void CreateButtons()
    {
        _objects.Buttons.GetComponent<Scenes>().TableText.text = "";
        var tmp = Variables.RoundList[Variables.Round];
        Variables.Amplitude = _amplitudes[tmp];
        Variables.ButtonWidthModifier = _buttonWidths[tmp];
        print("Amp" + Variables.Amplitude);
        print("Width" + Variables.ButtonWidthModifier);
        //creates circleAmount instances of the button prefab. Use an uneven number for Fitts Task
        Transform t;
        Variables.ButtonNames.Clear();
        for (var i = 0; i < CircleAmount; i++)
        {
            t = Instantiate(Btn, transform.localPosition, Quaternion.identity);
            t.SetParent(_objects.Buttons.GetComponent<Transform>());
            t.gameObject.name = "btn" + i;
            Variables.ButtonNames.Add(t.gameObject);
        }

        Variables.ButtonNames[0].GetComponentInChildren<Renderer>().material.color = Color.red;
        
        //Calculates the positions for the buttons on a circle
        if (Variables.ButtonNames.Count > 0 && gameObject != null)
        {
            for (var i = 0; i < Variables.ButtonNames.Count; i++)
            {
                var angleInDegrees = (i * (float) (360.0f / Variables.ButtonNames.Count));
                // Convert from degrees to radians via multiplication by Mathf.PI/180        
                var y = (float) (Radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180));
                var x = (float) (Radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180));
                Variables.ButtonNames[i].transform.localScale = new Vector3(
                    Variables.ButtonWidthModifier / _objects.MyTable.transform.localScale.x, 1,
                    Variables.ButtonWidthModifier / _objects.MyTable.transform.localScale.z);
                Variables.ButtonNames[i].transform.localPosition =
                    new Vector3(x * Variables.Amplitude, 0, y * Variables.Amplitude);
                Variables.ButtonNames[i].transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
        }
        Variables.ButtonNames[0].transform.position += new Vector3(0,0.002f,0);
        _objects.Panel.GetComponent<IgnoreCollision>()
            .IgnoreCollisionOf(_objects.TableTop.transform, _objects.Buttons.transform);

        Debug.Log("Round: " + Variables.Round + " start");
    }
    
    
}