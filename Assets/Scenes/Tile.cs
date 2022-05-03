
using System.Collections.Generic;

using UnityEngine;

public class Tile {

    public readonly int x;
    public readonly int y;
    public readonly int capacity;
    
    public int Sugar { get; private set; }
    public Tile North { get; private set; }
    public Tile South { get; private set; }
    public Tile East { get; private set; }
    public Tile West { get; private set; }

    public GameObject gameObject;
    public Agent agent;

    //Pathfinding
    //The cost to this node from the start node
    public int gCost;
    //The 'as the crow flies' cost from this node to the end
    public int hCost;
    //gCost + hCost
    public int fCost;
    public bool isWalkable;
    public Tile previousTile;

    /// <summary>
    /// Tile constructor
    /// </summary>
    /// <param name="x">The x-coord of the tile</param>
    /// <param name="y">The y-coord of the tile</param>
    /// <param name="capacity">The maximum amount of sugar possible on this tile</param>
    public Tile (int x, int y, int capacity) {
        this.x = x;
        this.y = y;
        if (Simulation.movementStyle == Simulation.MovementStyle.CUSTOM) {
            this.capacity = capacity * 50;
        } else {
            this.capacity = capacity;
        }
        Sugar = capacity;
        isWalkable = true;

        InitialiseLocation();
    }
    /// <summary>
    /// Sets this tile's neighbors, called on scape generation
    /// </summary>
    /// <param name="north">The north neighboring tile</param>
    /// <param name="south">The south neighboring tile</param>
    /// <param name="east">The east neighboring tile</param>
    /// <param name="west">The west neighboring tile</param>
    public void SetNeighbors (
        Tile north,
        Tile south,
        Tile east,
        Tile west
    ) {
        this.North = north;
        this.South = south;
        this.East = east;
        this.West = west;
    }

    /// <summary>
    /// Steps this tile, updates sugar value based on growback rate
    /// </summary>
    public void Step () {
        Sugar = Mathf.Min(Sugar + Simulation.growbackRate, capacity);
    }

    /// <summary>
    /// Renders the size of the sugar on the scape
    /// </summary>
    public void Render () {
        gameObject.transform.localScale = Mathf.Sqrt(Sugar) * Vector3.one / 25;
    }

    /// <summary>
    /// Removes sugar from this tile, returns the value of sugar to the agent
    /// </summary>
    /// <returns>The sugar gathered from this tile</returns>
    public int Gather (int sugarToTake) {
        int sugar = this.Sugar;
        if (sugarToTake < this.Sugar) {
            this.Sugar -= sugarToTake;
            return sugarToTake;
        } else {
            this.Sugar = 0;
            return sugar;
        }
    }

    /// <summary>
    /// Returns a matrix of possible locations, within a given range
    /// </summary>
    /// <param name="distance">The maximum distance where a tile is still included in the list</param>
    /// <returns>The matrix of possible locations</returns>
    public List<List<Tile>> GetAllLocationsInSight (int distance) {
        List<List<Tile>> allLocations = new List<List<Tile>>
        {
            GetNorthernLocations(distance),
            GetSouthernLocations(distance),
            GetEasternLocations(distance),
            GetWesternLocations(distance)
        };
        return allLocations;
    }

    /// <summary>
    /// Returns all tiles to the north of this tile, within the given range
    /// </summary>
    /// <param name="distance">Maximun distance to include a tile</param>
    /// <returns>A list of all possible northen tiles</returns>
    private List<Tile> GetNorthernLocations (int distance) {
        List<Tile> locations = new List<Tile>();
        Tile that = this;
        do {
            locations.Add(that.North);
            that = that.North;
        } while ( --distance > 0 );
        return locations;
    }

    /// <summary>
    /// Returns all tiles to the south of this tile, within the given range
    /// </summary>
    /// <param name="distance">Maximun distance to include a tile</param>
    /// <returns>A list of all possible southern tiles</returns>
    private List<Tile> GetSouthernLocations (int distance) {
        List<Tile> locations = new List<Tile>();
        Tile that = this;
        do {
            locations.Add(that.South);
            that = that.South;
        } while ( --distance > 0 );
        return locations;
    }

    /// <summary>
    /// Returns all tiles to the east of this tile, within the given range
    /// </summary>
    /// <param name="distance">Maximun distance to include a tile</param>
    /// <returns>A list of all possible eastern tiles</returns>
    private List<Tile> GetEasternLocations (int distance) {
        List<Tile> locations = new List<Tile>();
        Tile that = this;
        do {
            locations.Add(that.East);
            that = that.East;
        } while ( --distance > 0 );
        return locations;
    }

    /// <summary>
    /// Returns all tiles to the west of this tile, within the given range
    /// </summary>
    /// <param name="distance">Maximun distance to include a tile</param>
    /// <returns>A list of all possible western tiles</returns>
    private List<Tile> GetWesternLocations (int distance) {
        List<Tile> locations = new List<Tile>();
        Tile that = this;
        do {
            locations.Add(that.West);
            that = that.West;
        } while ( --distance > 0 );
        return locations;
    }

    /// <summary>
    /// Creates the unity Game Object for the sugar on the tile
    /// </summary>
    private void InitialiseLocation () {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gameObject.name = "Tile" + x + "," + y;
        //Set to 0, if sugar is present, this will increase
        gameObject.transform.localScale = new Vector3(0,0,0);
        gameObject.transform.localPosition = new Vector3(y, -0.95f, x);

        //As the simulation does not model physics, the collider is unnecessary overhead
        Object.Destroy(gameObject.GetComponent<Collider>());

        //Initialises Renderer
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.sharedMaterial = Materials.Sugar;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
    }

    public int ClassicGather () {
        int sugar = this.Sugar;
        this.Sugar = 0;
        return sugar;
    }

}
   