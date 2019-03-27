using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class SaveHandPosition
{
    private string[] _rowDataTemp;
    private GameObject GameController;
    private readonly GameObjects _objects;
    private static SaveHandPosition _instance = null;

    private static readonly string CsvSeparator = ",";

    public enum WhichHand
    {
        right,
        left
    }

    public WhichHand whichHand;
    string FileName;
    private StreamWriter sw;


    private SaveHandPosition()
    {
        Debug.Log("Starting Record boneData");
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
        return Application.dataPath + "/CSV/UserID" + Variables.UserId + "/UserID" + Variables.UserId + "_" + "bones_" +
               Variables.GetCurrentUnixTimestampMillis().ToString() + ".csv";
    }

    private string GetCsvHeader()
    {
        string header = "";

        //Sample, Time, SubjectID, GameObjectName, Hand, Texture, Displacement, Amp, Size, ID, Repetition, Duration, BoneName, PosX, PosY, PosZ, RotX, RotY, RotZ
        header += "Sample"+ CsvSeparator + "Time"+ CsvSeparator + "SubjectID"+ CsvSeparator + "GameObjectName"+
                  CsvSeparator + "Hand"+ CsvSeparator + "Texture"+ CsvSeparator + "Displacement"+ CsvSeparator +
                  "Amp"+ CsvSeparator + "Size"+ CsvSeparator + "ID"+ CsvSeparator + "Repetition"+ CsvSeparator +
                  "Duration"+ CsvSeparator + "BoneName"+ CsvSeparator + "PosX"+ CsvSeparator + "PosY"+
                  CsvSeparator + "PosZ"+ CsvSeparator + "RotX"+ CsvSeparator + "RotY"+ CsvSeparator + "RotZ";
        return header;
    }

    public static void Save(List<GameObject> bones, WhichHand w)
    {
        if (_instance == null)
        {
            _instance = new SaveHandPosition();
        }


        var timestamp = Variables.GetCurrentUnixTimestampMillis();
        var Sample = Variables.SampleNumber;
        var SubjectID = Variables.UserId;
        var GameObjectName = bones[0].name;
        var Hand = w;
        var Displacement = GameObjectName.Split('_')[3];
        var Texture = GameObjectName.Split('_')[4];
        var Amp = Variables.Amplitude;
        var Size = Variables.ButtonWidthModifier;
        var ID = Mathf.Log(Amp / (Size) + 0.5f, 2);
        var Repetition = SaveClickData._repetitionDict.ContainsKey(ID) ? SaveClickData._repetitionDict[ID] : 1;
        var Duration = timestamp - Variables.ClickTime;

        foreach (var bone in bones)
        {
            string output = "";
            var BoneName = bone.name;

            var PosX = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).x;
            var PosY = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).y;
            var PosZ = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).z;
            var RotX = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).x;
            var RotY = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).y;
            var RotZ = _instance._objects.Buttons.transform.InverseTransformPoint(bone.transform.position).z;

            output += Sample + CsvSeparator + timestamp + CsvSeparator + SubjectID + CsvSeparator + GameObjectName +
                      CsvSeparator + Hand + CsvSeparator + Texture + CsvSeparator + Displacement + CsvSeparator +
                      Amp + CsvSeparator + Size + CsvSeparator + ID + CsvSeparator + Repetition + CsvSeparator +
                      Duration + CsvSeparator + BoneName + CsvSeparator + PosX + CsvSeparator + PosY +
                      CsvSeparator + PosZ + CsvSeparator + RotX + CsvSeparator + RotY + CsvSeparator + RotZ;


            _instance.sw.Write(output + "\r\n");
            _instance.sw.Flush();
        }
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
        if (_instance != null)
        {
            _instance.sw.Close();
            Debug.Log("Finished Record boneData");
            _instance = null;
        }
    }
}