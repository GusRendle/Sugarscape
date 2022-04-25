using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStats {
    public List<int> agentWealth = new List<int>();
    public List<int> agentIncome = new List<int>();
    public List<int> agentVision = new List<int>();
    public List<int> agentMetab = new List<int>();
    public List<int> agentSpeed = new List<int>();
    public List<int>  agentPathLength = new List<int>();
    public List<float>  agentGreed = new List<float>();
    //MultiRun Stats
    public StepStat startStepStat;
    public StepStat stableStepStat;
    public StepStat dropStepStat;
    public StepStat endStepStat;

    public AgentStats()
    {
    }

    public void ImportAgentData(Agent agent) {
        agentWealth.Add(agent.wealth);
        agentIncome.Add(agent.income);
        agentVision.Add(agent.vision);
        agentMetab.Add(agent.metabolism);
        agentSpeed.Add(agent.speed);
        agentPathLength.Add(agent.lastPathLength);
        agentGreed.Add(agent.greed); 
    }
}
