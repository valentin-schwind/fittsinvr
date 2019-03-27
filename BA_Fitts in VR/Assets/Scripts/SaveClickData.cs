using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


public class SaveClickData
{
    private string[] _rowDataTemp;
    private GameObject GameController;
    private GameObjects _objects;
    private static SaveClickData instance = null;
    public static Dictionary<float, int> _repetitionDict = new Dictionary<float, int>();
    public static readonly string CSV_SEPARATOR = ",";


    string FileName;
    StreamWriter sw;


    private SaveClickData()
    {
        Debug.Log("Starting Record");
        this.FileName = GetPath();
        FileInfo f = new FileInfo(FileName);
        if (f.Directory != null) f.Directory.Create();
        bool writeHeader = !File.Exists(FileName);
        GameController = GameObject.Find("GameController");
        _objects = GameController.GetComponent<GameObjects>();

        sw = new StreamWriter(FileName, true);
        if (writeHeader)
        {
            sw.Write(GetCsvHeader() + "\r\n");
            sw.Flush();
        }
    }

    private static string GetPath()
    {
        return Application.dataPath + "/CSV/UserID" + Variables.UserId + "/UserID" + Variables.UserId + "_" +
               Variables.GetCurrentUnixTimestampMillis().ToString() + ".csv";
    }

    private string GetCsvHeader()
    {
        string header = "";

        header +=
            "Sample" + CSV_SEPARATOR + "Time" + CSV_SEPARATOR + "SubjectID" + CSV_SEPARATOR + "GameObject" +
            CSV_SEPARATOR + "Texture" + CSV_SEPARATOR + "Displacement" + CSV_SEPARATOR + "ID" + CSV_SEPARATOR + "Size" +
            CSV_SEPARATOR + "Amp" + CSV_SEPARATOR + "Duration" + CSV_SEPARATOR + "TargetNo" + CSV_SEPARATOR + "Hit" + CSV_SEPARATOR + "Repetition" +
            CSV_SEPARATOR + "Throughput" + CSV_SEPARATOR + "posTargetX" + CSV_SEPARATOR + "posTargetZ" + CSV_SEPARATOR +
            "posFingerX" + CSV_SEPARATOR + "posFingerZ" + CSV_SEPARATOR + "Distance" + CSV_SEPARATOR + "LastRoundFailed";


        return header;
    }

    public static void Save()
    {
        if (instance == null)
        {
            instance = new SaveClickData();
        }

        string output = "";

        var sample = Variables.SampleNumber;
        Variables.SampleNumber++;
        
        var unixTIme = Variables.GetCurrentUnixTimestampMillis();

        var SubjectID = Variables.UserId;
        var TargetNo = Variables.ButtonNames[Variables.NextIndex];
        var CurrentHandGameObject =
            instance._objects.HandController.GetComponent<GetHandMovement>().GetCurrentHand().name;
        var displacement = CurrentHandGameObject.Split('_')[3];
        var texture = CurrentHandGameObject.Split('_')[4];

        var amplitude = Variables.Amplitude;

        var buttonWidth = Variables.ButtonWidthModifier;

        var ID = Mathf.Log(amplitude / (buttonWidth) + 0.5f, 2);

       


        bool Hit = !Variables.ButtonMissedFlag;
        if (Hit)
        {
            if (_repetitionDict.ContainsKey(ID))
            {
                _repetitionDict[ID]++;
            }
            else _repetitionDict.Add(ID, 1); 
        }
       

        var Repetition = (int)(_repetitionDict[ID] / 16) + 1 ;


        var duration = Hit ? Variables.Duration: Variables.MissedTimeSinceLastHit;

        var throughput = Throughput.CalculateTp(ID, Variables.DurationArray.ToArray());
        var posTargetX = TargetNo.transform.localPosition.x;
        var posTargetZ = TargetNo.transform.localPosition.z;

        var posFingerX = Variables.ClickPositionX;
        var posFingerZ = Variables.ClickPositionY;

        var distance = Mathf.Sqrt(Mathf.Pow(posTargetX - posFingerX, 2) + Mathf.Pow(posTargetZ - posFingerZ, 2));
        
        

        output += sample + CSV_SEPARATOR + unixTIme + CSV_SEPARATOR + SubjectID + CSV_SEPARATOR +
                  CurrentHandGameObject + CSV_SEPARATOR + texture + CSV_SEPARATOR + displacement + CSV_SEPARATOR + ID +
                  CSV_SEPARATOR + buttonWidth + CSV_SEPARATOR + amplitude + CSV_SEPARATOR + duration + CSV_SEPARATOR +
                  TargetNo.name + CSV_SEPARATOR + Hit + CSV_SEPARATOR + Repetition + CSV_SEPARATOR + throughput + CSV_SEPARATOR + posTargetX +
                  CSV_SEPARATOR + posTargetZ + CSV_SEPARATOR + posFingerX + CSV_SEPARATOR + posFingerZ +
                  CSV_SEPARATOR + distance + CSV_SEPARATOR + Variables.LastRoundFailed;


        instance.sw.Write(output + "\r\n");
        instance.sw.Flush();
    }


    // parse left hand rotation


    public static string FloatToString(float value)
    {
        return String.Format("{0:0.####################}", value);
    }

    public static string DoubleToString(double value)
    {
        return String.Format("{0:0.####################}", value);
    }

    //call if study is over
    public static void closeIfOpen()
    {
        if (instance != null)
        {
            instance.sw.Close();
            Debug.Log("Finished Record");
            instance = null;
        }
    }
}