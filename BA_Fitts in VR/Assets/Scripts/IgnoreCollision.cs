using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

	public void IgnoreCollisionOf(Transform t1, Transform t2)
    {
        var c = t2.GetComponentsInChildren<Collider>();
        foreach (var col in c)
        {
            Physics.IgnoreCollision(t1.GetComponent<Collider>(), col);
        }
        
    }
}
