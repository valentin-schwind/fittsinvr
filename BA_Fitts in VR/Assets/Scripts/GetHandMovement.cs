using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.ManusVR.Scripts;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.PlayerLoop;

public class GetHandMovement : MonoBehaviour
{
    public GameObject GameController;
    private GameObjects _objects;

    //Bones from Manus Hands
    public GameObject[] ManusBonesRight;

    public GameObject[] ManusBonesLeft;

    //The current Bones of the Leapmotion Hands
    public List<GameObject> LeapBonesRight = new List<GameObject>();

    public List<GameObject> LeapBonesLeft = new List<GameObject>();

    //List of All Hand Models
    public List<GameObject> RightHandList = new List<GameObject>();

    public List<GameObject> LeftHandList = new List<GameObject>();

    //LeapBones of all Leaphands
    public Dictionary<string, List<GameObject>> RightHands = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<GameObject>> LeftHands = new Dictionary<string, List<GameObject>>();

    private int _index = 0;
    private string _indexR;
    private string _indexL;

    private Calibration _calib;
    private Quaternion _lastRightArmRot;
    private Quaternion _currentRightArmRot;
    private readonly float[] _distR = new float[6];
    private readonly float[] _distL = new float[6];
    private GameObject _thisLight;
    private Light _light;

    private enum WhichHand
    {
        Right,
        Left
    }

    private WhichHand _whichHand;

    private void Awake()
    {
        _distR[0] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_05").transform.position);
        _distR[1] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_04").transform.position);
        _distR[2] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_03").transform.position);
        _distR[3] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_02").transform.position);
        _distR[4] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_01").transform.position);
        _distR[5] = Vector3.Distance(GameObject.Find("R_Palm").transform.position,
            GameObject.Find("R_Forearm_meta").transform.position);

        _distL[0] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_05").transform.position);
        _distL[1] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_04").transform.position);
        _distL[2] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_03").transform.position);
        _distL[3] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_02").transform.position);
        _distL[4] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_01").transform.position);
        _distL[5] = Vector3.Distance(GameObject.Find("L_Palm").transform.position,
            GameObject.Find("L_Forearm_meta").transform.position);
    }


    private void Start()
    {
        _objects = GameController.GetComponent<GameObjects>();
        _calib = GameController.GetComponent<Calibration>();

        _thisLight = GameObject.Find("Directional light");
        _light = _thisLight.GetComponent<Light>();
        //get indexes to calc distances to bones
        _indexR = RightHandList[_index].name;
        _indexL = LeftHandList[_index].name;
        

        foreach (var g in RightHandList)
        {
            AddHand(g.transform, g.name, WhichHand.Right);
        }

        foreach (var g in LeftHandList)
        {
            AddHand(g.transform, g.name, WhichHand.Left);
        }

        _indexR = RightHandList[_index].name;
        _indexL = LeftHandList[_index].name;
        LeapBonesRight = RightHands[_indexR];
        LeapBonesLeft = LeftHands[_indexL];
        AddCollider(_objects.IndexCollider);

        //Set first Hands active
        RightHandList[_index].SetActive(true);
        LeftHandList[_index].SetActive(true);

        foreach (var r in _objects.ManusVR.GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }

    public void StartWithStudy()
    {
        RightHandList[_index].SetActive(false);
        LeftHandList[_index].SetActive(false);
        //get new order with latinsquare
        GameObject[] Rtmp = GetLatinSquare(RightHandList.ToArray(), Variables.UserId);
        RightHandList = Rtmp.ToList();

        GameObject[] Ltmp = GetLatinSquare(LeftHandList.ToArray(), Variables.UserId);
        LeftHandList = Ltmp.ToList();
        _indexR = RightHandList[_index].name;
        _indexL = LeftHandList[_index].name;
        LeapBonesRight = RightHands[_indexR];
        LeapBonesLeft = LeftHands[_indexL];
        RightHandList[_index].SetActive(true);
        LeftHandList[_index].SetActive(true);
    }
    public GameObject GetCurrentHand()
    {
        return RightHandList[_index];
    }

    // Hände array rein -> hände reihenfolge raus
    

    private void AddCollider(GameObject indexFinger)
    {
        indexFinger.AddComponent<CapsuleCollider>();
        indexFinger.GetComponent<CapsuleCollider>().center = new Vector3(-0.007f, 0, 0.001f);
        indexFinger.GetComponent<CapsuleCollider>().radius = 0.008f;
        indexFinger.GetComponent<CapsuleCollider>().height = 0.03f;
        indexFinger.GetComponent<CapsuleCollider>().direction = 0;
    }

    private void AddHand(Transform t, string _name, WhichHand whichHand)
    {
        var allChildren = t.GetComponentsInChildren<Transform>();
        var childObjects = allChildren.Select(child => child.gameObject).ToList();
        switch (whichHand)
        {
            case WhichHand.Right:
                RightHands.Add(_name, childObjects);
                break;
            case WhichHand.Left:
                LeftHands.Add(_name, childObjects);
                break;
            default:
                throw new ArgumentOutOfRangeException("whichHand", whichHand, null);
        }
    }
    //Used to turn off lights on Toon hand and turn it on for the others
    public void CheckIfFlatHand()
    {
        var texture = RightHandList[_index].name.Split('_')[4];

        if (texture == "toon")
        {
            _light.shadows = LightShadows.None;
        }
        else
        {
            _light.shadows = LightShadows.Soft;
        }
    }

    private void Update()
    {
        CheckIfFlatHand();
    }

    public void NextHand()
    {
        RightHandList[_index].SetActive(false);
        LeftHandList[_index].SetActive(false);

        _index = (_index + 1) % RightHandList.Count;
        _indexR = RightHandList[_index].name;
        _indexL = LeftHandList[_index].name;

        LeapBonesRight = RightHands[_indexR];
        LeapBonesLeft = LeftHands[_indexL];
        RightHandList[_index].SetActive(true);
        LeftHandList[_index].SetActive(true);
        
    }

    private void PrevHand()
    {
        RightHandList[_index].SetActive(false);
        LeftHandList[_index].SetActive(false);
        //todo
        _index = (_index - 1) % RightHandList.Count;
        _indexR = RightHandList[_index].name;
        _indexL = LeftHandList[_index].name;

        LeapBonesRight = RightHands[_indexR];
        LeapBonesLeft = LeftHands[_indexL];
        RightHandList[_index].SetActive(true);
        LeftHandList[_index].SetActive(true);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(_calib.NextHand))
        {
            NextHand();
        }

        if (Input.GetKeyDown(_calib.Prevhand))
        {
            PrevHand();
        }

        // For Hand size Calibration - Redo //todo 
//        if (Input.GetKeyDown(_calib.HandsizeCalibration))
//        {
//            _calib.CalibrateHandsize();
//            foreach (var g in RightHandList)
//            {
//                g.GetComponent<Transform>().localScale = _objects.Right.transform.localScale;
//            }
//
//            foreach (var g in LeftHandList)
//            {
//                g.GetComponent<Transform>().localScale = _objects.Left.transform.localScale;
//            }
//        }

        MoveRightHand();
        MoveLefttHand();
        MoveIndexCol();
    }


    private void MoveIndexCol()
    {
        _objects.IndexCollider.transform.position = LeapBonesRight[11].transform.position;
        _objects.IndexCollider.transform.rotation = LeapBonesRight[11].transform.rotation;
    }

    private void MoveRightHand()
    {

        //Hand postion, thumb position
        LeapBonesRight[2].GetComponent<Transform>().position = ManusBonesRight[0].GetComponent<Transform>().position;
        LeapBonesRight[21].GetComponent<Transform>().position = ManusBonesRight[23].GetComponent<Transform>().position;

        //Hand rotation
        LeapBonesRight[2].GetComponent<Transform>().rotation =
            ManusBonesRight[0].GetComponent<Transform>().rotation * Quaternion.Euler(180, 90, -90);


        //Arm Rotation and Pos

        LeapBonesRight[3].transform.rotation = ManusBonesRight[1].transform.rotation * Quaternion.Euler(180, 0, -90);
        LeapBonesRight[3].transform.position =
            ManusBonesRight[1].transform.position + _distR[5] * ManusBonesRight[1].transform.up;

        for (var i = 0; i < 5; i++)
        {
            LeapBonesRight[i + 4].transform.rotation =
                LeapBonesRight[3].transform.rotation * Quaternion.Euler(i * 18, 0, 0);
            ;
            LeapBonesRight[i + 4].transform.position =
                ManusBonesRight[1].transform.position + _distR[4 - i] * ManusBonesRight[1].transform.up;
        }

        //Finger Rotation
        for (var i = 0; i < 3; i++)
        {
            //Index
            LeapBonesRight[i + 9].GetComponent<Transform>().rotation =
                ManusBonesRight[i + 3].GetComponent<Transform>().rotation * Quaternion.Euler(90, 180, 0);
            //Middle
            LeapBonesRight[i + 12].GetComponent<Transform>().rotation =
                ManusBonesRight[i + 8].GetComponent<Transform>().rotation * Quaternion.Euler(90, 180, 0);
            //Pinky
            LeapBonesRight[i + 15].GetComponent<Transform>().rotation =
                ManusBonesRight[i + 13].GetComponent<Transform>().rotation * Quaternion.Euler(90, 180, 0);
            //Ring
            LeapBonesRight[i + 18].GetComponent<Transform>().rotation =
                ManusBonesRight[i + 18].GetComponent<Transform>().rotation * Quaternion.Euler(90, 180, 0);
            //Thumb
            LeapBonesRight[i + 21].GetComponent<Transform>().rotation =
                ManusBonesRight[i + 23].GetComponent<Transform>().rotation * Quaternion.Euler(0, 180, 0);
        }
        if (Variables.isTrialActive) SaveHandPosition.Save(LeapBonesRight, SaveHandPosition.WhichHand.right);
    }

    private void MoveLefttHand()
    {
        //Hand postion, thumb position
        LeapBonesLeft[2].GetComponent<Transform>().position = ManusBonesLeft[0].GetComponent<Transform>().position;
        LeapBonesLeft[21].GetComponent<Transform>().position = ManusBonesLeft[23].GetComponent<Transform>().position;

        //Hand rotation
        LeapBonesLeft[2].GetComponent<Transform>().rotation =
            ManusBonesLeft[0].GetComponent<Transform>().rotation * Quaternion.Euler(180,90,90);


        //Arm Rotation and Pos

        LeapBonesLeft[3].transform.rotation = ManusBonesLeft[1].transform.rotation * Quaternion.Euler(0,-90,-90);
        LeapBonesLeft[3].transform.position =
            ManusBonesLeft[1].transform.position + _distL[5] * ManusBonesLeft[1].transform.up;

        for (var i = 0; i < 5; i++)
        {
            LeapBonesLeft[i + 4].transform.rotation =
                LeapBonesLeft[3].transform.rotation * Quaternion.Euler(-(i * 18), 0, 0);
            ;
            LeapBonesLeft[i + 4].transform.position =
                ManusBonesLeft[1].transform.position + _distL[4 - i] * ManusBonesLeft[1].transform.up;
        }

        //Finger Rotation
        for (var i = 0; i < 3; i++)
        {
            //Index
            LeapBonesLeft[i + 9].GetComponent<Transform>().rotation =
                ManusBonesLeft[i + 3].GetComponent<Transform>().rotation * Quaternion.Euler(-90, 180, 0);
            //Middle
            LeapBonesLeft[i + 12].GetComponent<Transform>().rotation =
                ManusBonesLeft[i + 8].GetComponent<Transform>().rotation * Quaternion.Euler(-90, 180, 0);
            //Pinky
            LeapBonesLeft[i + 15].GetComponent<Transform>().rotation =
                ManusBonesLeft[i + 13].GetComponent<Transform>().rotation * Quaternion.Euler(-90, 180, 0);
            //Ring
            LeapBonesLeft[i + 18].GetComponent<Transform>().rotation =
                ManusBonesLeft[i + 18].GetComponent<Transform>().rotation * Quaternion.Euler(-90, 180, 0);
            //Thumb
            LeapBonesLeft[i + 21].GetComponent<Transform>().rotation =
                ManusBonesLeft[i + 23].GetComponent<Transform>().rotation * Quaternion.Euler(0, 0, 180);
        }
        if (Variables.isTrialActive) SaveHandPosition.Save(LeapBonesRight, SaveHandPosition.WhichHand.left);
    }
    public T[] GetLatinSquare<T>(T[] array, int participant)

    {
        // 1. Create initial square.

        int[,] latinSquare = new int[array.Length, array.Length];


        // 2. Initialise first row.

        latinSquare[0, 0] = 1;

        latinSquare[0, 1] = 2;


        for (int i = 2, j = 3, k = 0; i < array.Length; i++)

        {
            if (i % 2 == 1)

                latinSquare[0, i] = j++;

            else

                latinSquare[0, i] = array.Length - (k++);
        }


        // 3. Initialise first column.

        for (int i = 1; i <= array.Length; i++)

        {
            latinSquare[i - 1, 0] = i;
        }


        // 4. Fill in the rest of the square.

        for (int row = 1; row < array.Length; row++)

        {
            for (int col = 1; col < array.Length; col++)

            {
                latinSquare[row, col] = (latinSquare[row - 1, col] + 1) % array.Length;


                if (latinSquare[row, col] == 0)

                    latinSquare[row, col] = array.Length;
            }
        }


        T[] newArray = new T[array.Length];


        for (int col = 0; col < array.Length; col++)

        {
            int row = (participant + 1) % (array.Length);

            newArray[col] = array[latinSquare[row, col] - 1];
        }

        return newArray;
    }
}