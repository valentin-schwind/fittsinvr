using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Slider for Answers for Questionnaire
public class CreateSlider : MonoBehaviour
{
    public GameObject GameController;
    private GameObjects _objects;
    public Transform Btn;
    //private TablePosition _tablePos;
    public Dictionary<GameObject,int> Answers = new Dictionary<GameObject,int>();
    private Material _material;
    // Use this for initialization
    private void Start () {
        GameController = GameObject.Find("GameController");
        _objects = GameController.GetComponent<GameObjects>();
        SetupSlider(7, _objects.TableTop.transform.localScale.z);
	}
    
    private void SetupSlider(int amount, float size)
    {
        for (var i = 0; i < amount; i++)
        {
            var t = Instantiate(Btn, Vector3.zero, Quaternion.identity);
            t.SetParent(transform);
            t.gameObject.name = "btn" + i;
            t.GetComponentInChildren<Renderer>().material.color = Color.white;
            Answers.Add(t.gameObject, i+1);
            Variables.Answers.Add(t.gameObject);
        }
        foreach (var a in Answers)
        {
            a.Key.transform.localPosition = new Vector3(a.Value * (size * 0.9f / amount) - size/2,0,0 );
            a.Key.transform.localScale = new Vector3(5, 1, 5);
        }
    }

}
