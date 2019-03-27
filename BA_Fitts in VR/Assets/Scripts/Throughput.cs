using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Throughput
{
    private static Throughput instance = null;
    private Throughput()
    {
        if (instance == null)
        {
            instance = new Throughput();
        }
    }

    public static float CalculateTp(float id, float[] mt)
    {
        return (id / (mt.Sum()/mt.Length)) *1000;
    }
}
