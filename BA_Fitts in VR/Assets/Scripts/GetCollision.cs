using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollision : MonoBehaviour
{
    public GameObject GameController;
    private GameObjects _objects;

    private void Start()
    {
        GameController = GameObject.Find("GameController");
        _objects = GameController.GetComponent<GameObjects>();
    }

    public bool MyCollisionDetection()
    {
        var tableTop = _objects.TableTop.transform.position + _objects.TableTop.transform.localScale / 2;
        return (GetComponent<CapsuleCollider>().ClosestPoint(tableTop).y < tableTop.y);
    }
}