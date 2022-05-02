using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

// Enum for diffrent movement styles
    public enum MovementStyle {
        CLASSIC,
        CUSTOM
    }

    public static int growbackRate = 61;
    public static int initialAgentCount = 400;
    public static class Wealth {
        public static int min = 50;
        public static int max = 100;
    }
    public static class Vision {
        public static int min = 1;
        public static int max = 6;
    }
    public static class Metabolism {
        public static int min = 1;
        public static int max = 4;
    }
    public static class AgentSpeed {
        public static int min = 1;
        public static int max = 4;
    }
    public static class Greed {
        public static float min = 0f;
        public static float max = 0.5f;
    }
    
    public static MovementStyle movementStyle = MovementStyle.CLASSIC;
    public static bool MultiRun = false;

    public static int CurrentStep { get; private set; } = 0;

    public static List<Agent> liveAgents;

    public static Sugarscape sugarscape;
    public static List<Agent> agents;

    //Stats
    public static List<StepStats> stepsStats = new List<StepStats>();
    private static AgentsStats agentStats;
    //UI Stats
    public static string stepCountStats;
    public static string agentCountStats;
    public static string greedStats;
    public static string incomeStats;
    public static string metabStats;

    /// <summary>
    /// Initialises / resets the simulation
    /// </summary>
    public static void Initialise () {
        CurrentStep = 0;
        stepsStats.Clear();
        agentStats = new AgentsStats();
        stepCountStats = "";
        agentCountStats = "";
        greedStats = "";
        incomeStats = "";
        metabStats = "";
        growbackRate = Mathf.CeilToInt((float)(initialAgentCount * 2.925)/24);
        InitialiseSugarscape();
        InitialiseAgents();
        Render();
    }

    /// <summary>
    /// Runs each 'step' of the simulation
    /// </summary>
    /// <returns>False if the simulation has finished</returns>
    public static bool Step () {
        liveAgents = agents.FindAll(x => x.IsAlive);
        if ( liveAgents.Count == 0  ) {
            //All agents are dead
            return false;
        }

        //Reset Stat Lists
        agentStats = new AgentsStats();

        int one =0;
            int tw =0;
            int th =0;
            int fo =0;
            int fi =0;
            int si =0;

        //Randomly goes calls for each agent to handle the step
        foreach ( Agent agent in Main.Shuffle(liveAgents)) {
            agentStats.ImportAgentData(agent);
            agent.Step();
            switch (agent.speed)
            {
                case 1:
                    one++;
                    break;
                case 2:
                    tw++;
                    break;
                case 3:
                    th++;
                    break;
                case 4:
                    fo++;
                    break;
                case 5:
                    fi++;
                    break;
                case 6:
                    si++;
                    break;
            }
        } 
        
        if (CurrentStep < 601)
        {
            sugarscape.Step();
        }

        //Update Stats
        if (CurrentStep % 10 == 0) {
            stepCountStats += "\nStep " + CurrentStep.ToString();
            agentCountStats += "\n" + liveAgents.Count;
            greedStats += "\n" + agentStats.agentGreed.Average().ToString("0.000");
            incomeStats += "\n" + agentStats.agentIncome.Average().ToString("0.000");
            metabStats += "\n" + agentStats.agentMetab.Average().ToString("0.000");
        }

        if (MultiRun) {
            if (Simulation.CurrentStep == 0) {
                growbackRate = 74;
                //growbackRate = Mathf.CeilToInt((float)(initialAgentCount * 2.925)/24);
            }  else if (Simulation.CurrentStep == 300) {
                growbackRate = 26;
                //growbackRate = Mathf.CeilToInt((float)(0.5f * ((liveAgents.Count * 2.925)/24)));
            }  
            // else if (Simulation.CurrentStep == 899) {
            //    foreach ( Agent agent in Main.Shuffle(liveAgents)) {
            //         switch (agent.speed)
            //         {
            //             case 1:
            //                 one++;
            //                 break;
            //             case 2:
            //                 tw++;
            //                 break;
            //             case 3:
            //                 th++;
            //                 break;
            //             case 4:
            //                 fo++;
            //                 break;
            //             case 5:
            //                 fi++;
            //                 break;
            //             case 6:
            //                 si++;
            //                 break;
            //         }
            //     }
            //     float max = (float) liveAgents.Count;
            //     string stat = "";
            //     stat += (one/max).ToString() + "/n";
            //     stat += (tw/max).ToString() + "/n";
            //     stat += (th/max).ToString() + "/n";
            //     stat += (fo/max).ToString() + "/n";
            //     stat += (fi/max).ToString() + "/n";
            //     stat += (si/max).ToString();
            //     stat += "/n";
            // }
        }

        stepsStats.Add(new StepStats(CurrentStep, agentStats));
        CurrentStep++;
        return true;
    }

    /// <summary>
    /// handles rendering of the sugarscape and agents
    /// </summary>
    public static void Render () {
        sugarscape.Render();
        // Renders all alive agents
        foreach ( Agent agent in agents.FindAll(x => x.IsAlive) ) {
            agent.Render();
        }
    }
    
    /// <summary>
    /// Initialises the Sugarscape object for the simulation
    /// </summary>
    private static void InitialiseSugarscape () {
        //Destroys previous sugarscape and their gameobjects (in case of scape regeneration)
        if ( sugarscape != null ) {
            sugarscape.Destroy();
        }
        sugarscape = new Sugarscape();
    }

    /// <summary>
    /// Initialises all agents on the sugarscape
    /// </summary>
    private static void InitialiseAgents () {
        //Destroys previous agents and their gameobjects (in case of scape regeneration)
        if ( agents != null ) {
            foreach ( Agent agent in agents ) {
                agent.Destroy();
            }
        }
        agents = new List<Agent>();
        for ( int i = 0 ; i < initialAgentCount ; i++ ) {
            //Links each agent to an unoccupied location
            Tile tile = sugarscape.GetUnoccupiedTile();
            Agent agent = new Agent(tile);
            tile.agent = agent;
            agents.Add(agent);
        }
    }

}  
