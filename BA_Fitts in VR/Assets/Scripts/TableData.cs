using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class TableData
{

    public float FrontLeftX;
    public float FrontLeftY;
    public float FrontLeftZ;

    public float FronRightX;
    public float FronRightY;
    public float FronRightZ;

    public float TopLeftX;
    public float TopLeftY;
    public float TopLeftZ;

    public float Height;

    public TableData(float frontLeftXFloat, float frontLeftYFloat, float frontLeftZFloat,
        float fronRightXFloat, float fronRightYFloat, float fronRightZFloat,
        float topLeftXFloat, float topLeftYFloat, float topLeftZFloat,
        float heightFloat)
    {
        FrontLeftX = frontLeftXFloat;
        FrontLeftY = frontLeftYFloat;
        FrontLeftZ = frontLeftZFloat;

        FronRightX = fronRightXFloat;
        FronRightY = fronRightYFloat;
        FronRightZ = fronRightZFloat;

        TopLeftX = topLeftXFloat;
        TopLeftY = topLeftYFloat;
        TopLeftZ = topLeftZFloat;
        Height = heightFloat;

    }
}
