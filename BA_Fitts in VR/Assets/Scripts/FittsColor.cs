using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FittsColor : MonoBehaviour
{
    private static FittsColor _fittsInstance;
    private GameObject _next;
    [Header("InteractionBehaviour Colors")]
    public Color MyRed = new Color32(0xFF, 0x00, 0x00, 0xFF); // RGBA
    public Color MyWhite = new Color32(0xFF, 0xFF, 0xFF, 0xFF); // RGBA
    private Material _material;

    private void Awake()
    {
        _fittsInstance = this;
    }

    private void Start()
    {
        var renderer = transform.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        if (renderer != null)
        {
            _material = renderer.material;
        }
    }

    private void OnButtonPressAction()
    {
        if (Variables.Times != null)
        {
            if (Variables.Times.Count == Variables.Count)
            {
                Variables.Count++;

                if (Variables.Count == Variables.ButtonNames.Count)
                {
                    Scenes.ScenesInstance.NextRound();
                }
            }
        }

        if (Variables.Count == 1)
        {
            Variables.MinusTime = Variables.GetCurrentUnixTimestampMillis();
        }

        if (Variables.Times != null)
        {
            float hitTime = Variables.GetCurrentUnixTimestampMillis() - Variables.MinusTime;
            Variables.ClickTime = Variables.GetCurrentUnixTimestampMillis();
            Variables.Duration = (Variables.Times.Count > 0) ? hitTime - Variables.Times[Variables.Times.Count - 1] : 0; 
            Variables.DurationArray.Add(Variables.Duration);
            Variables.Times.Add(hitTime);

        }
        //get the next button to become red
        Variables.ThisIndex = Variables.NextIndex;
        Variables.NextIndex = (Variables.NextIndex + ((Variables.ButtonNames.Count + 1) / 2)) % Variables.ButtonNames.Count;
        _next = Variables.ButtonNames[Variables.NextIndex];
        ChangeColor(MyWhite, gameObject);
        gameObject.GetComponentInChildren<MyCollisionDetection>().enabled = false;
        ChangeColor(MyRed, _next);
        _next.GetComponentInChildren<MyCollisionDetection>().enabled = true;
        gameObject.transform.position += new Vector3(0,-0.002f,0);
        _next.transform.position += new Vector3(0,0.002f,0); 
        GetComponent<MyCollisionDetection>().isButtonPressed = false;

    }
    
    public void OnButtonPress()
    {
        if (_material != null)
        {
            if (_material.color == MyRed)
            {
                print(transform.parent.gameObject.name+"Button Pressed");
                SaveClickData.Save();
                Variables.ClickPositions.Add(Variables.ClickPosition);
                OnButtonPressAction();
            }
        }

    }

    //function for changing the color of a GameObject
    private static void ChangeColor(Color color, GameObject g)
    {
        g.GetComponentInChildren<Renderer>().material.color = color;
    }
}
