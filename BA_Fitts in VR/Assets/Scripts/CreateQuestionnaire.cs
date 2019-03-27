using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateQuestionnaire : MonoBehaviour
{
	public GameObject GameController;
	private GameObjects _objects;
	public Transform question;
	public string [] Questions;

	private TablePosition tp;
	private int _questionAmount;
// Use this for initialization
	private void Awake()
	{
		_questionAmount = Questions.Length;
	}

	void Start ()
	{
		_objects = GameController.GetComponent<GameObjects>();
		_objects.Panel.GetComponent<IgnoreCollision>().IgnoreCollisionOf(_objects.TableTop.transform,transform);
		
	}

	private void OnEnable()
	{
		//CreateQuestions();
	}

	public void CreateQuestions()
	{
		float[] pos = {25, -6, -37};
		for (int i = 0; i < _questionAmount; i++)
		{
			
			Transform t = Instantiate(question, transform.position, Quaternion.identity);
			t.name = "Question " + (i + 1);
			t.SetParent(transform);
			t.GetComponent<Transform>().localScale = new Vector3(1,1,1);
			t.GetComponent<Transform>().localRotation = Quaternion.Euler(0,0,0);
			t.GetComponent<Transform>().localPosition = new Vector3(0,pos[i],0);
			t.GetComponentInChildren<Question>().SetText(Questions[i]);
			t.GetChild(0).transform.localScale = new Vector3(0.1f -i*0.01f, 0.1f-i*0.01f, 0.1f-i*0.01f);
			Variables.AnswerDict.Add(t.gameObject,0);
			
			
		}

	}
}
