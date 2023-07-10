using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RunningWaterSystem2D
{
    private static List<RunningWater2D> runningWater2Ds;

    static RunningWaterSystem2D()
    {
        runningWater2Ds = new List<RunningWater2D>();
    }

    public static void Add(RunningWater2D runningWater2D)
    {
        runningWater2Ds.Add(runningWater2D);
    }

    public static List<RunningWater2D> Get()
    {
        return runningWater2Ds;
    }

    public static void Remove(RunningWater2D runningWater2D)
    {
        runningWater2Ds.Remove(runningWater2D);
    }
}