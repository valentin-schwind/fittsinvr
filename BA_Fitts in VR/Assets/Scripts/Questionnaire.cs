using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Questionnaire : MonoBehaviour
{
    public GameObject Questionnaire1;
    public GameObject Questionnaire2;
    private string[] _output;
    public GameObject GameController;
    private CreateSlider _createSlider;
    public void SetActiveQ(bool isActive, GameObject Q)
    {
        Q.SetActive(isActive);
        if (isActive == false && Variables.Answers.Count != 0)
        {
            DestroyButtons();
            Variables.Answers.Clear();
        }

    }

    private void Start()
    {
        SetActiveQ(false, Questionnaire1);
        SetActiveQ(false, Questionnaire2);
    }
    public void DestroyButtons()
    {
        print("destroy");
        foreach (GameObject g in Variables.Answers)
        {
            Destroy(g);
        }
    }
}