using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollisionDetection : MonoBehaviour
{
    public GameObject GameController;
    private GameObjects _objects;
    public GameObject Btn;

    private float _radius;
    public float Offset;


    public ButtonClass ButtonClass;
    private GameObject _mySlider;
    private CreateSlider _slider;

    private bool isButtonMissed = false;
    [NonSerialized]public bool isButtonPressed = false;


    public enum ButtonType
    {
        Fitts,
        Answer,
        Confirm
    }

    public ButtonType Buttontype;

    private void Start()
    {
        GameController = GameObject.Find("GameController");
        Btn = transform.parent.gameObject;
        _objects = GameController.GetComponent<GameObjects>();
        ButtonClass = transform.parent.GetComponent<ButtonClass>();
        if (Buttontype == ButtonType.Fitts)
        {
            if (GetComponent<Renderer>().material.color != Color.red) GetComponent<MyCollisionDetection>().enabled = false; 
        }
    }

    private void Update()
    {
    
        
        //testen wenn RE
        if (Variables.isCollisionEnabled)
        {
            var clickPos = _objects.IndexCollider.GetComponent<CapsuleCollider>()
                .ClosestPoint(Btn.transform.position);
            var localCLick = _objects.Buttons.transform.InverseTransformPoint(clickPos);
            if (!(Btn.transform.position.y - clickPos.y + Offset > 0 ||
                Btn.transform.position.y - clickPos.y - Offset > 0))
            {
                Variables.ButtonMissedFlag = false;
                isButtonMissed = false;
                //isButtonPressed is set false in FittsColor
                
            }
            
            if (ButtonClass.GetRadius() >
                GetDistanceToButtonMid(Btn, _objects.IndexCollider.GetComponent<CapsuleCollider>()))
            {
                if (Btn.transform.position.y - clickPos.y + Offset > 0 ||
                    Btn.transform.position.y - clickPos.y - Offset > 0)
                {
                    if (!Variables.ButtonMissedFlag)
                    {
                        Variables.ClickPositionX = localCLick.x;
                        Variables.ClickPositionY = localCLick.z;
                        switch (Buttontype)
                        {
                            case ButtonType.Fitts:
                                if (!isButtonMissed && !isButtonPressed) ButtonPressed();
                                break;
                            case ButtonType.Answer:
                                AnswerPressed();
                                break;
                            case ButtonType.Confirm:
                                ConfirmPressed(transform.parent.transform.parent.gameObject);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            if (ButtonClass.GetRadius() + 0.03f >
                     GetDistanceToButtonMid(Btn, _objects.IndexCollider.GetComponent<CapsuleCollider>()) &&
                     Buttontype == ButtonType.Fitts)
            {
                if (Btn.transform.position.y - clickPos.y + Offset > 0 ||
                    Btn.transform.position.y - clickPos.y - Offset > 0)
                {
                    if (!Variables.ButtonMissedFlag)
                    {
                        Variables.ClickPositionX = localCLick.x;
                        Variables.ClickPositionY = localCLick.z;
                        Variables.ButtonMissedFlag = true;
                        if (!isButtonMissed && !isButtonPressed) ButtonMissed();
                    }
                }
            }

            if (ButtonClass.GetRadius() + 0.03f <
                GetDistanceToButtonMid(Btn, _objects.IndexCollider.GetComponent<CapsuleCollider>()))
            {
                if (Btn.transform.position.y - clickPos.y + Offset > 0 ||
                    Btn.transform.position.y - clickPos.y - Offset > 0)
                {
                    isButtonMissed = true;
                    Variables.ButtonMissedFlag = true;
                }
            }
        }
    }

    private void ButtonPressed()
    {
        isButtonPressed = true;
        gameObject.GetComponent<FittsColor>().OnButtonPress();
    }

    private void ButtonMissed()
    {
        isButtonMissed = true;
        Variables.ClickTime = Variables.GetCurrentUnixTimestampMillis();
        Variables.MissedTimeSinceLastHit = (Variables.Times.Count > 0)
            ? (Variables.GetCurrentUnixTimestampMillis() - Variables.MinusTime) -
              Variables.Times[Variables.Times.Count - 1]
            : 0;

        if (Variables.MissedTimeSinceLastHit >= 30)
        {
            print(transform.parent.gameObject.name+"Button Missed");
            SaveClickData.Save();
        }
    }

    private void AnswerPressed()
    {
        _mySlider = gameObject.transform.parent.transform.parent.gameObject;
        _slider = _mySlider.GetComponent<CreateSlider>();
        int _nextRed = (_slider.Answers[gameObject.transform.parent.gameObject]);
        foreach (var g in _slider.Answers.Keys)
        {
            g.GetComponentInChildren<Renderer>().material.color = Color.white;
        }

        gameObject.GetComponent<QuestionnaireColor>().OnAnswerPress();
        Variables.AnswerDict[transform.parent.transform.parent.transform.parent.gameObject] = _nextRed;
    }

    private void ConfirmPressed(GameObject g)
    {
        if (Variables.AnswerDict.ContainsValue(0))
        {
            print("Please answer every question!");
        }
        else
        {
            g.GetComponent<Countdown>().StartCountdown(3.9f);
            //_objects.Confirm.GetComponent<Countdown>().StartCountdown(5.9f);
        }
    }

    private float GetDistanceToButtonMid(GameObject button, Collider indexCollider)
    {
        var btnPos = button.transform.position;
        var colliderPos = indexCollider.ClosestPoint(btnPos);
        var ab = btnPos - colliderPos;
        var distance = Mathf.Sqrt(ab.x * ab.x + ab.z * ab.z);
        return distance;
    }
}