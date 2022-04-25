
using System.Collections.Generic;
using System.Linq;

public class Simulation {

// Enum for diffrent movement styles
    public enum MovementStyle {
        CLASSIC,
        CUSTOM
    }

    public static int growbackRate = 43;
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
        public static int max = 3;
    }
    public static class Lifespan {
        public static int min = 60;
        public static int max = 100;
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
    public static List<StepStat> stepStatList = new List<StepStat>();
    public static AgentStats agentStats;
    //UI Stats
    public static string stepsStats;
    public static string agentCountStats;
    public static string greedStats;
    public static string incomeStats;
    public static string metabStats;

    /// <summary>
    /// Initialises / resets the simulation
    /// </summary>
    public static void Initialise () {
        CurrentStep = 0;
        stepStatList.Clear();
        agentStats = new AgentStats();
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
        if ( liveAgents.Count == 0 ) {
            //All agents are dead
            return false;
        }
        //Reset Stat Lists
        agentStats.agentGreed.Clear();
        agentStats.agentIncome.Clear();
        agentStats.agentMetab.Clear();


        //Randomly goes calls for each agent to handle the step
        foreach ( Agent agent in Main.Shuffle(liveAgents)) {
            agentStats.ImportAgentData(agent);
            agent.Step();
        } 
        sugarscape.Step();

        //Update Stats
        if (CurrentStep % 10 == 0) {
            stepsStats += "\nStep " + CurrentStep.ToString();
            agentCountStats += "\n" + liveAgents.Count;
            greedStats += "\n" + agentStats.agentGreed.Average().ToString("0.000");
            incomeStats += "\n" + agentStats.agentIncome.Average().ToString("0.000");
            metabStats += "\n" + agentStats.agentMetab.Average().ToString("0.000");
        }

        if (MultiRun) {
            if (Simulation.CurrentStep == 0) {
                agentStats.startStepStat = GenerateStepStat();
                growbackRate = 43;
            }  else if (Simulation.CurrentStep == 300) {
                agentStats.stableStepStat = GenerateStepStat();
                growbackRate = 22;
            } else if (Simulation.CurrentStep == 600) {
                agentStats.dropStepStat = GenerateStepStat();
            } else if (Simulation.CurrentStep == 900) {
                agentStats.endStepStat = GenerateStepStat();
            }
        }

        stepStatList.Add(GenerateStepStat());
        CurrentStep++;
        return true;
    }

    private static StepStat GenerateStepStat() {
        return new StepStat(CurrentStep, liveAgents.Count, (float) agentStats.agentWealth.Average(), (float) agentStats.agentIncome.Average(),
            (float) agentStats.agentVision.Average(), (float) agentStats.agentMetab.Average(), (float) agentStats.agentSpeed.Average(), 
            (float) agentStats.agentPathLength.Average(), (float) agentStats.agentGreed.Average());
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
