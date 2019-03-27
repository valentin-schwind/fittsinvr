using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
	public Text _text;
	public void SetText(string question)
	{
		_text.text = question;
	}
}
