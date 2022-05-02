using System.Linq;

[System.Serializable]
public class StepStats
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
    
    public StepStats(int step, AgentsStats agentStats)
    {
        Step = step;
        AgentCount = agentStats.agentWealth.Count();
        AvgWealth = (float) agentStats.agentWealth.Average();
        AvgIncome = (float) agentStats.agentIncome.Average();
        AvgVision = (float) agentStats.agentVision.Average();
        AvgMetab = (float) agentStats.agentMetab.Average();
        AvgSpeed = (float) agentStats.agentSpeed.Average();
        AvgPath = (float) agentStats.agentPathLength.Average();
        AvgGreed = agentStats.agentGreed.Average();
    }
}