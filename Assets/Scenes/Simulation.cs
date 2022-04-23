
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

    public static int CurrentStep { get; private set; } = 0;

    public static Sugarscape sugarscape;
    public static List<Agent> agents;

    //Stats
    public static string stepsStats;
    public static string agentStats;
    public static string greedStats;
    public static string incomeStats;
    public static string metabStats;

    private static List<float> agentGreed = new List<float>();
    private static List<int> agentIncome = new List<int>();
    private static List<int> agentMetab = new List<int>();

    /// <summary>
    /// Initialises / resets the simulation
    /// </summary>
    public static void Initialise () {
        CurrentStep = 0;
        InitialiseSugarscape();
        InitialiseAgents();
        Render();
    }

    /// <summary>
    /// Runs each 'step' of the simulation
    /// </summary>
    /// <returns>False if the simulation has finished</returns>
    public static bool Step () {
        List<Agent> liveAgents = agents.FindAll(x => x.IsAlive);
        if ( liveAgents.Count == 0 ) {
            //All agents are dead
            return false;
        }
        //Reset Stat Lists
        agentGreed.Clear();
        agentIncome.Clear();
        agentMetab.Clear();


        //Randomly goes calls for each agent to handle the step
        foreach ( Agent agent in Main.Shuffle(liveAgents)) {
            agentGreed.Add(agent.greed);
            agentIncome.Add(agent.income);
            agentMetab.Add(agent.metabolism);
            agent.Step();
        } 
        sugarscape.Step();

        //Update Stats
        if (CurrentStep % 10 == 0) {
            stepsStats += "\nStep " + CurrentStep.ToString();
            agentStats += "\n" + liveAgents.Count;
            greedStats += "\n" + agentGreed.Average().ToString("0.000");
            incomeStats += "\n" + agentIncome.Average().ToString("0.000");
            metabStats += "\n" + agentMetab.Average().ToString("0.000");
        }

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
