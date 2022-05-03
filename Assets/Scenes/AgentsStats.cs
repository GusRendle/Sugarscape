using System.Collections.Generic;

public class AgentsStats {
    public List<float> agentWealth = new List<float>();
    public List<float> agentIncome = new List<float>();
    public List<int> agentVision = new List<int>();
    public List<int> agentMetab = new List<int>();
    public List<int> agentSpeed = new List<int>();
    public List<int>  agentPathLength = new List<int>();
    public List<float>  agentGreed = new List<float>();

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
