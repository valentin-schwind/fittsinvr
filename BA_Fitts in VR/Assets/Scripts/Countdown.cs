using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text _text;
    private bool goCountdown = false;
    private float time = 1;
    private string _name;
    private bool isColliderOn = true;


    private void Update()
    {
        if (goCountdown)
        {
            time -= Time.deltaTime;
            _text.text = "" + Mathf.Round(time);
            if (isColliderOn)
            {
                Variables.isCollisionEnabled = false;
                isColliderOn = false;
            }


            if (time <= 0)
            {
                if (!isColliderOn)
                {
                    Variables.isCollisionEnabled = true;
                    isColliderOn = true;
                }

                _name = transform.parent.gameObject.name;
                switch (_name)
                {
                    case "Fragebogen":
                        Variables.IsConfirmPressed = true;

                        break;
                    case "VorFrage":
                        Variables.IsConfirmPREPressed = true;
                        break;
                }

                goCountdown = false;
                _text.text = "";
            }
        }
    }

    public void StartCountdown(float timeLeft)
    {
        goCountdown = true;
        time = timeLeft;
    }
}