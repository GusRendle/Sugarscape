
using System.Collections.Generic;

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
    public static class Lifespan {
        public static int min = 60;
        public static int max = 100;
    }
    
    public static MovementStyle movementStyle = MovementStyle.CLASSIC;

    public static int CurrentStep { get; private set; } = 0;

    public static Sugarscape sugarscape;
    public static List<Agent> agents;

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
        //Randomly goes calls for each agent to handle the step
        foreach ( Agent agent in Main.Shuffle(liveAgents)) {
            agent.Step();
        } 
        sugarscape.Step();
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
