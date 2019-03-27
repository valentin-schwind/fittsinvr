using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using Assets.ManusVR.Scripts;

public class Scenes : MonoBehaviour
{
    public GameObject GameController;
    private GameObjects _objects;
    public static Scenes ScenesInstance;
    public Text TableText;
    
    private void Start()
    {
        _objects = GameController.GetComponent<GameObjects>();
        TableText.text = "Your objective is to touch the red buttons as fast and precise as possible. ";
    }

    private void Awake()
    {
        ScenesInstance = this;
    }


    public void NextRound()
    {
        //Debug.Log(Variables.Trial);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var g in Variables.ButtonNames)
        {
            Destroy(g);
        }

        Variables.LastRoundFailed = false;
        Variables.Count = 1;
        Variables.NextIndex = 0;
        Variables.Times.Clear();
        Variables.Round = (Variables.Round + 1) % (Variables.AmountOfRounds);
        if (Variables.Round == 0)
        {
            //Close last HandData save
            SaveHandPosition.closeIfOpen();
            Variables.isTrialActive = false;
            //Wait after last trial and show timer
            float elapsedTime = 5.0f;
            while (elapsedTime > 0.0f)
            {
                elapsedTime -= Time.deltaTime;
                TableText.text = "End of Task \n " + Mathf.Round(elapsedTime);
                if (Input.GetKeyDown(KeyCode.Escape)) elapsedTime = 0.0f;
                else yield return null;
            }
            TableText.text = "";
            _objects.TableTop.GetComponent<Questionnaire>().SetActiveQ(true, _objects.Fragebogen);
            _objects.Fragebogen.GetComponent<CreateQuestionnaire>().CreateQuestions();
            while (Variables.IsConfirmPressed == false)
            {
                yield return null;
            }

            if (Variables.IsConfirmPressed)
            {
                SaveQuestionnaire.SaveCollecter(SaveQuestionnaire.QuestionnaireType.Post);
                _objects.TableTop.GetComponent<Questionnaire>().SetActiveQ(false, _objects.Fragebogen);
                Variables.IsConfirmPressed = false;
                print("Trial: " + Variables.Trial + " von " + Variables.AmountOfTrials);
                if (Variables.Trial == Variables.AmountOfTrials -1)
                {
                    float endTime = 120.0f;
                    while (endTime > 0.0f)
                    {
                        endTime -= Time.deltaTime;
                        TableText.text = "End of Study \n Thanks for participating!";
                        if (Input.GetKeyDown(KeyCode.Escape)) endTime = 0.0f;
                        else yield return null;
                    }
                    TableText.text = "";
                }
                Variables.Trial++;
                SaveClickData._repetitionDict.Clear();
                _objects.Buttons.GetComponent<Setup>().ShuffleList(Variables.UserId, Variables.Trial);
                _objects.HandController.GetComponent<GetHandMovement>().NextHand();
                yield return StartCoroutine(CreatePreQuestionnaire());
                
            }
        }

        Debug.Log("Round: " + Variables.Round + " von " + Variables.AmountOfRounds);
        yield return new WaitForSeconds(1);
        Setup.SetupInstance.CreateButtons();
    }

    
    public IEnumerator CreatePreQuestionnaire()
    {
        //Wait for 60 seconds or break with Keypress
        TableText.text = "";
        print("Vorfragebogen");
        float elapsedTime = 45.0f;
        while (elapsedTime > 0.0f)
        {
            elapsedTime -= Time.deltaTime;
            TableText.text = "Make yourself familiar with your new hands \n  " + Mathf.Round(elapsedTime);
            if (Input.GetKeyDown(KeyCode.Escape)) elapsedTime = 0.0f;
            else yield return null;
        }

        TableText.text = "";
        _objects.TableTop.GetComponent<Questionnaire>().SetActiveQ(true, _objects.PreFrage);
        _objects.PreFrage.GetComponent<CreateQuestionnaire>().CreateQuestions();
        while (Variables.IsConfirmPREPressed == false)
        {
            yield return null;
        }

        if (Variables.IsConfirmPREPressed)
        {
            SaveQuestionnaire.SaveCollecter(SaveQuestionnaire.QuestionnaireType.Pre);
            // _objects.TableTop.GetComponent<Questionnaire>().SaveQuestionnaire();
            _objects.TableTop.GetComponent<Questionnaire>().SetActiveQ(false, _objects.PreFrage);
            Variables.isTrialActive = true;
           
            Variables.IsConfirmPREPressed = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RestartRound();
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            for (var i = 0; i < 15; i++)
            {
                Variables.Times.Add(0);
                Variables.ClickPositions.Add(new Vector2(0, 0));
            }

            NextRound();
        }
    }
    private void RestartRound()
    {
        Variables.LastRoundFailed = true;
        foreach (var g in Variables.ButtonNames)
        {
            Destroy(g);
        }
        Variables.Count = 1;
        Variables.NextIndex = 0;
        Variables.Times.Clear();
        Debug.Log("Round: " + Variables.Round);
        Setup.SetupInstance.CreateButtons();
    }

   
}