using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireColor : MonoBehaviour
{
    private GameObject _mySlider;
    private CreateSlider _slider;

    public Color MyRed = new Color32(0xFF, 0x00, 0x00, 0xFF); // RGBA
    public Color MyWhite = new Color32(0xFF, 0xFF, 0xFF, 0xFF); // RGBA
    private Material _material;
    private int _nextRed;

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

        _material.color = MyWhite;
    }

    public void OnAnswerPress()
    {
        if (_material != null)
        {
            _material.color = MyRed;

        }
    }
}