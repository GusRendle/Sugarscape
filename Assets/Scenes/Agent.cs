
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Agent {

    //Values used in agent generation
    private static int staticId = 0;
    public int id;
    public readonly int vision;
    public readonly int metabolism;

    //Values used throughout simulation
    public bool IsAlive { get; private set; } = true;
    //The agent's accumulated sugar stores
    public int SugarStore { get; private set; }
    //Agent's current location
    public Tile tile;

    public GameObject gameObject;
    public Renderer renderer;

    //Pathfinding
    private List<Tile> path;
    private readonly Tile home;

    private readonly int income;
    private int wealth;
    private readonly int speed =1;
    private readonly float greed = 0.1f;

    /// <summary>
    /// Default agent constructor
    /// </summary>
    /// <returns>The Agent object</returns>
    public Agent (Tile tile) : this(
        Random.Range(Simulation.Wealth.min, Simulation.Wealth.max + 1),
        Random.Range(Simulation.Vision.min, Simulation.Vision.max + 1),
        Random.Range(Simulation.Metabolism.min, Simulation.Metabolism.max + 1),
        tile
    ) { }

    /// <summary>
    /// Arg agent constructor
    /// </summary>
    /// <param name="income">Income of the agent</param>
    /// <param name="vision">Vision of the agent</param>
    /// <param name="metabolism">Metabolism of the agent</param>
    public Agent (int income, int vision, int metabolism, Tile home) {
        this.income = income;
        this.vision = vision;
        this.metabolism = metabolism;
        this.tile = home;
        this.home = home;
        id = staticId++;
        SugarStore = metabolism * 20;
        wealth = income * 5;
        InitialiseAgent();
    }

    /// <summary>
    /// Manage's agents actions per step
    /// </summary>
    public void Step () {
        wealth += income;
        Move();
        Eat();
        if ( SugarStore < 0 ) {
            Die();
            return;
        }
    }

    /// <summary>
    /// Renders the agent based on the current view
    /// </summary>
    public void Render () {
        //Renders agent at it's location
        gameObject.transform.localPosition = new Vector3(tile.y, 0, tile.x);
        //Based on the current view, agents are rendered differently
        switch ( UI.currentView ) {
            case UI.ViewOptions.DEFAULT:
                renderer.sharedMaterial = Materials.DefaultColour;
                break;
            case UI.ViewOptions.SEX:
                //renderer.sharedMaterial = sex == Sex.MALE ? Materials.MALE : Materials.FEMALE;
                break;
            case UI.ViewOptions.VISION:
                //The max and min vision values for the simulation are split into thirds, and which third the agent belongs to determins it's colour
                {
                    float firstTercile = GetFirstTercile(Simulation.Vision.min, Simulation.Vision.max);
                    float secondTercile = GetSecondTercile(Simulation.Vision.min, Simulation.Vision.max);
                    if ( vision < firstTercile ) {
                        renderer.sharedMaterial = Materials.LowColour;
                    } else if ( vision <= secondTercile ) {
                        renderer.sharedMaterial = Materials.MedColour;
                    } else {
                        renderer.sharedMaterial = Materials.HighColour;
                    }
                }
                break;
            case UI.ViewOptions.METABOLISM:
                //Like vision, possible metabolism values are also split into thirds
                {
                    float firstTercile = GetFirstTercile(Simulation.Metabolism.min, Simulation.Metabolism.max);
                    float secondTercile = GetSecondTercile(Simulation.Metabolism.min, Simulation.Metabolism.max);
                    if ( metabolism < firstTercile ) {
                        renderer.sharedMaterial = Materials.LowColour;
                    } else if ( metabolism <= secondTercile ) {
                        renderer.sharedMaterial = Materials.MedColour;
                    } else {
                        renderer.sharedMaterial = Materials.HighColour;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Gets the first Tercile (value between the 1st and 2nd thirds)
    /// </summary>
    /// <param name="min">Minimum value of the range</param>
    /// <param name="max">Maximum value of the range</param>
    /// <returns></returns>
    private float GetFirstTercile (int min, int max) {
        return min + (max - min) / 3f;
    }

    /// <summary>
    /// Gets the second Tercile (value between the 2nd and 3rd thirds)
    /// </summary>
    /// <param name="min">Minimum value of the range</param>
    /// <param name="max">Maximum value of the range</param>
    /// <returns></returns>
    private float GetSecondTercile (int min, int max) {
        return min + (max - min) / 3f * 2;
    }

    private void Move () {
        //The location the agent will move to next
        Tile nextLocation = null;
        //The sugar value of this location
        int nextSugar = -1;
        //The location the agent is currently evaluating 
        Tile potentialLocation = null;
        //The sugar value of this location
        int potentialSugar = -1;
        //All locations the agent can see are shuffled, as per the movement rule
        List<List<Tile>> allPotentialLocations = Main.Shuffle(tile.GetAllLocationsInSight(vision));
        //Agents move differently depending on the current movement rule
        switch ( Simulation.movementStyle ) {
            case Simulation.MovementStyle.CLASSIC:
                {
                    for ( int i = 0 ; i < vision ; i++ ) {
                        for ( int j = 0 ; j < 4 ; j++ ) {
                            //As the locations start from 0, in the case of equal sugar amounts, the closest is chosen.
                            potentialLocation = allPotentialLocations[j][i];
                            //Checks location for other agent
                            if ( potentialLocation.agent != null ) {
                                continue;
                            }
                            potentialSugar = potentialLocation.Sugar;
                            //If new location has more sugar than current best, set new location to current best
                            if ( nextLocation == null || potentialSugar > nextSugar ) {
                                nextSugar = potentialSugar;
                                nextLocation = potentialLocation;
                            }
                        }
                    }
                }
                break;
            case Simulation.MovementStyle.CUSTOM:
            //VISION = 49
                {
                    if (path == null || path.Count == 0) {
                        //Agent is searching for sugar
                        path = Pathfinding.FindPath(tile, Pathfinding.FindClosestSugar(tile));
                        nextLocation = path[0];
                        path.RemoveAt(0);
                    } else {
                        //Agent is already moving
                        for (int i = 0; i < speed; i++) {
                            nextLocation = path[0];
                            path.RemoveAt(0);
                            if (path.Count == 0 ) {
                                //At destination
                                if (nextLocation != home) {
                                    //If the destination is not home, set next journey to return home
                                    path = Pathfinding.FindPath(tile, home);
                                    Gather(nextLocation, path.Count);
                                } else {
                                    //Destination is home
                                }
                                break;
                            }
                        }

                        
                    }
                }
                break;
        }
        //Removes the agent from the current location, and adds it to the new location
        if ( nextLocation != null ) {
            tile.agent = null;
            tile = nextLocation;
            tile.agent = this;
        }
    }

    private void Gather (Tile nextLocation, int pathLength) {
        //Calculates the sugar the agent needs to collect to survive + sugar taken due to greed
        int sugarToTake = Mathf.CeilToInt((metabolism * (pathLength + 2) * 2 * (1 + greed)));
        int gathered = nextLocation.Gather(sugarToTake);
        SugarStore += gathered;

    }

    /// <summary>
    /// Each step, the sugar stores are depleted by the amount the agent needs to survive.
    /// If the agent eats sugar it doesn't have, it dies.
    /// </summary>
    private void Eat () {
        SugarStore -= metabolism;
    }

    /// <summary>
    /// On complete lack of sugar, agent is killed and removed from the simulation
    /// </summary>
    private void Die () {
        IsAlive = false;
        tile.agent = null;
        Destroy();
    }

    /// <summary>
    /// Destroys the agent
    /// </summary>
    public void Destroy () {
        Object.Destroy(gameObject);
    }

    /// <summary>
    /// Creates the unity Game Object for the agent
    /// </summary>
    private void InitialiseAgent () {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = "Agent" + id;
        //Sets the size of the agent on the scape
        gameObject.transform.localScale = new Vector3(0.9f,0.9f,0.9f);
        //As the simulation does not model physics, the collider is unnecessary overhead
        Object.Destroy(gameObject.GetComponent<Collider>());

        //Initialises Renderer
        renderer = gameObject.GetComponent<Renderer>();
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.sharedMaterial = Materials.DefaultColour;
    }

}
