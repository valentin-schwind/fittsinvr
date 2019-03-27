using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveTablePos
{
    private void Start()
    {
        SaveFile();
        LoadFile();
    }
    public void SaveFile()
    {
       //var destination = Application.persistentDataPath + "/save2.dat";
        var destination = "Assets/save2.dat";

        var file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);

        var data = new TableData(Variables.TopFrontLeft.x, Variables.TopFrontLeft.y, Variables.TopFrontLeft.z,
            Variables.TopFrontRight.x, Variables.TopFrontRight.y, Variables.TopFrontRight.z,
            Variables.TopBackLeft.x, Variables.TopBackLeft.y, Variables.TopBackLeft.z,
            Variables.TableHeight);
        var bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        //var destination = Application.persistentDataPath + "/save2.dat";
        var destination = "Assets/save2.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }
        
        var bf = new BinaryFormatter();
        var data = (TableData)bf.Deserialize(file);

        file.Close();

        Variables.TopFrontLeft = new Vector3(data.FrontLeftX, data.FrontLeftY, data.FrontLeftZ);
        Variables.TopFrontRight = new Vector3(data.FronRightX, data.FronRightY, data.FronRightZ);
        Variables.TopBackLeft = new Vector3(data.TopLeftX, data.TopLeftY, data.TopLeftZ);
        Variables.TableHeight = data.Height;
        Debug.Log("loaded Table Data");
    }

}