using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ManusVR.Scripts;
using UnityEditor;
using UnityEditorInternal;
using Valve.VR;

public class Calibration : MonoBehaviour
{
    private GameObjects _objects;

    private float _distance;
    
    public KeyCode HandsizeCalibration = KeyCode.M;
    public KeyCode TableSizeCalibration = KeyCode.H;
    public KeyCode PlaceButtons = KeyCode.B;

    public KeyCode NextHand = KeyCode.RightArrow;
    public KeyCode Prevhand = KeyCode.LeftArrow;

    private float _measuredLeapHandSize = 16.8f; //Size of Leap Hand from Indexfinger to Wrist
    public float MeasuredHandSize;
    private GetCollision _col;
    private bool _defaultIsSet;
    public bool CalibFlag = true;

    private void Start()
    {
        _objects = GetComponent<GameObjects>();
        _col = _objects.IndexCollider.GetComponent<GetCollision>();
        
        print("1. Press 'M' to calibrate Handsize with Finger standing on Table");
        print("2. Hold 'H' to calibrate Tableheight while Hand in Touch position");
        print("3. Press 'B' to start the Study.");

        
    }

//    private float GetHandSize()
//    {
//        var WristPos = _objects.R_Palm.transform.position;
//        return (WristPos.y - Variables.TableHeight);
//
//    }
    
    //this function calculates the Size of the Leap hand from indexfinger to Wrist
    private void GetLeapHandSize()
    {
        var WristPos = _objects.R_Palm.transform.position;
        var IndexFingerPos = _objects.IndexCollider.GetComponent<Collider>()
            .ClosestPoint(_objects.TableTop.transform.position);
        var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
        c.transform.position = IndexFingerPos;
        c.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        
        var c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        c2.transform.position = WristPos;
        c2.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        
        print(Vector3.Distance(WristPos, IndexFingerPos));
    }
    private void CalibrateHandSize()
    {
//        var indexCollider = _objects.IndexCollider.GetComponent<CapsuleCollider>();
//        var tableTopCollider = _objects.TableTop.GetComponent<BoxCollider>();
//        
//        var cpToIndexFinger = tableTopCollider.ClosestPoint(indexCollider.transform.position);
//        
//        var cpToTableTop = indexCollider.ClosestPoint(cpToIndexFinger);
//        
//        _distance = cpToTableTop.y - cpToIndexFinger.y;
//        Debug.Log(_distance);

        _distance = ( MeasuredHandSize / _measuredLeapHandSize);    //difference between real hand and leap hand
        print(_distance);
        foreach (var g in _objects.HandController.GetComponent<GetHandMovement>().RightHandList)
        {
            g.transform.localScale = new Vector3(_distance, _distance, _distance);
        }
        foreach (var g in _objects.HandController.GetComponent<GetHandMovement>().LeftHandList)
        {
            g.transform.localScale = new Vector3(_distance, _distance, _distance);
        }

         _objects.IndexCollider.GetComponent<CapsuleCollider>().height *= _distance;

    }

    private void MoveTableTop()
    {
        float tmp = 0;
        if (!_col.MyCollisionDetection())
        {
            tmp = 0.0001f;
            _objects.MyTable.GetComponent<TablePosition>()
                .SetTableHeight(tmp);
        }
        else
        {
            tmp = -0.0001f;
            _objects.MyTable.GetComponent<TablePosition>()
                .SetTableHeight(tmp);
        }
        _objects.MyTable.GetComponent<TablePosition>()
            .SetTableHeight(tmp);
    }

    private void Update()
    {
        if (CalibFlag)
        {
            if (Input.GetKeyDown(HandsizeCalibration))
            {
                CalibrateHandSize();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                GetLeapHandSize();
            }
            if (Input.GetKeyDown(PlaceButtons))
            {
                Setup.SetupInstance.StartSetup();
                _objects.HandController.GetComponent<GetHandMovement>().StartWithStudy();
                CalibFlag = false;
            }
        }
        if (Input.GetKey(TableSizeCalibration))
        {
            MoveTableTop();
        }
        

    }

}