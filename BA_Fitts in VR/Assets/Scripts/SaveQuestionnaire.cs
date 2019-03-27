using System;
using System.IO;
using UnityEngine;

public class SaveQuestionnaire 
{
    private string[] _rowDataTemp;
    private GameObject _gameController;
    private GameObjects _objects;
    private SaveClickData _saveClickDataInstance;
    private static SaveQuestionnaire _instance = null;

    public enum QuestionnaireType
    {
        Pre,
        Post
    }

    public QuestionnaireType questionnaireType;
    public static readonly string CsvSeparator = ",";


    string _fileName;
    StreamWriter _sw;


    private SaveQuestionnaire()
    {
        Debug.Log("Starting Record_Questionnaire");
        this._fileName = GetPath();
        FileInfo f = new FileInfo(_fileName);
        if (f.Directory != null) f.Directory.Create();
        bool writeHeader = !File.Exists(_fileName);
        _gameController = GameObject.Find("GameController");
        _objects = _gameController.GetComponent<GameObjects>();

        _sw = new StreamWriter(_fileName, true);
        if (writeHeader)
        {
            _sw.Write(GetCsvHeader() + "\r\n");
            _sw.Flush();
        }
    }

    private static string GetPath()
    {
        return Application.dataPath + "/CSV/UserID" + Variables.UserId + "/UserID" + Variables.UserId + "_" + "Questionnaire_" + 
               Variables.GetCurrentUnixTimestampMillis().ToString() + ".csv";
    }

    private string GetCsvHeader()
    {
        string header = "";
        header += "SubjectID"+ CsvSeparator +"Time"+ CsvSeparator + "QuestionID"+ CsvSeparator + "Rating"+ CsvSeparator + "Displacement"+ CsvSeparator + "Texture"+ CsvSeparator + "GameObject";
        return header;
    }

    public static void SaveCollecter(QuestionnaireType type)
    {
        int index = 1;
        switch (type)
        {
            case QuestionnaireType.Pre:
                index = 1;
                break;
            case QuestionnaireType.Post:
                index = 4;
                break;
        }
        int[] values = new int[Variables.AnswerDict.Values.Count];
        Variables.AnswerDict.Values.CopyTo(values, 0);
        for (int i = 0; i < Variables.AnswerDict.Count; i++)
        {
            Save(index, values[i]);
            index++;
        }
        Variables.AnswerDict.Clear();
    }

    private static void Save(int questionID, int r)
    {
        if (_instance == null)
        {
            _instance = new SaveQuestionnaire();
        }

        string output = "";

        var subjectId = Variables.UserId;
        var time = Variables.GetCurrentUnixTimestampMillis();
        var questionId = questionID;
        var rating = r;
        var currentHandGameObject =
            _instance._objects.HandController.GetComponent<GetHandMovement>().GetCurrentHand().name;
        var displacement = currentHandGameObject.Split('_')[3];
        var texture = currentHandGameObject.Split('_')[4];
        output += subjectId+ CsvSeparator +time+ CsvSeparator + questionId+ CsvSeparator + rating+ CsvSeparator + displacement+ CsvSeparator + texture+ CsvSeparator + currentHandGameObject;
        _instance._sw.Write(output + "\r\n");
        _instance._sw.Flush();
    }




    public static string FloatToString(float value)
    {
        return String.Format("{0:0.####################}", value);
    }

    public static string DoubleToString(double value)
    {
        return String.Format("{0:0.####################}", value);
    }

    //call if study is over
    public static void CloseIfOpen()
    {
        if (_instance != null)
        {
            _instance._sw.Close();
            Debug.Log("Finished Record_Questionnaire");
            _instance = null;
        }
    }
}