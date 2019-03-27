using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClass : MonoBehaviour
{
    public float GetRadius()
    {
        return (transform.GetChild(0).GetComponent<Renderer>().bounds.size.x / 2);
    }

}
