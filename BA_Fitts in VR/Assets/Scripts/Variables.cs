using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Assets.ManusVR.Scripts.ManusInterface;

//Class for global variable and function declaration
public class Variables : MonoBehaviour
{
    //Variables for Fitts Setup
    public static int UserId;
    public static float Amplitude;
    public static float ButtonWidthModifier;

    //Variables for Rounds and Trials
    public static int[] RoundList;
    public static int Round = 0;
    public static int AmountOfRounds;
    public static int AmountOfTrials;
    public static int Trial = 0;
    public static int Count = 1;
    public static int NextIndex = 0;
    public static int ThisIndex = 0;
    public static float MinusTime;
    
    public static readonly List<float> Times = new List<float>();
    public static readonly List<GameObject> ButtonNames = new List<GameObject>();


    public static Vector2 ClickPosition;
    public static readonly List<Vector2> ClickPositions = new List<Vector2>();
    public static float ClickPositionX;
    public static float ClickPositionY;

    public static Dictionary<GameObject, int> AnswerDict = new Dictionary<GameObject, int>();

    //Table position
    public static Vector3 TopFrontLeft;
    public static Vector3 TopFrontRight;
    public static Vector3 TopBackLeft;
    public static float TableHeight;
    private static readonly System.DateTime UnixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    public static float Duration = 0;
    public static float MissedTimeSinceLastHit = 0;
    public static float ClickTime = GetCurrentUnixTimestampMillis();
    public static List<float> DurationArray = new List<float>();
    
    public static bool isCollisionEnabled = true;
    //public static CapsuleCollider IndexCol;
    public static readonly List<string[]> RowData = new List<string[]>();

    public static float WristXAlignmentDefault = 0.0f;
    public static float WristXAlignment = 0.0f;

    public static List<GameObject> Answers = new List<GameObject>();

    public static int SampleNumber = 0;

    private static string _question1 = "I have the sensation to feel present in the virtual space.";
    private static string _question2 = "It seems like my own hands are located in the virtual world.";
    private static string _question3 = "I feel as if the hands in the virtual world are my own hands.";
    private static string _question4 = "I was able to interact with the enviroment the way I wanted to.";
    private static string _question5 = "I had the sensation that the touch I felt on my hands matched the touch I saw using my virtual hands.";
    private static string _question6 = "It seemed as if touching with the virtual hands resembled touching with my own hands.";

    public static bool LastRoundFailed = false;

    public static bool isTrialActive = false;
    
    public static bool ButtonMissedFlag = false;
    public static bool IsConfirmPressed = false;
    public static bool IsConfirmPREPressed = false;
    
    public static long GetCurrentUnixTimestampMillis()
    {
        return (long)(System.DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
    }


}
