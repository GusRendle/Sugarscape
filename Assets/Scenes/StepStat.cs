using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StepStat
{
    public int Step;
    public int AgentCount;
    public float AvgWealth;
    public float AvgIncome;
    public float AvgVision;
    public float AvgMetab;
    public float AvgSpeed;
    public float AvgPath;
    public float AvgGreed;

    public StepStat(int step, int agentCount, float avgWealth, float avgIncome, float avgVision, float avgMetab, float avgSpeed, float avgPath, float avgGreed)
    {
        Step = step;
        AgentCount = agentCount;
        AvgWealth = avgWealth;
        AvgIncome = avgIncome;
        AvgVision = avgVision;
        AvgMetab = avgMetab;
        AvgSpeed = avgSpeed;
        AvgPath = avgPath;
        AvgGreed = avgGreed;
    }
}