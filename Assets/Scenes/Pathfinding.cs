using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    //private const int MOVE_DIAGONAL_COST = 14;

    //The list of tiles that are possible paths
    private static List<Tile> openList;
    //The list of nodes that have been searched / are unavailable
    private static List<Tile> closedList;

    /// <summary>
    /// Finds a path given a start and end node
    /// </summary>
    /// <param name="startTile">The tile the node originates from</param>
    /// <param name="endTile">The ending tile for the path</param>
    /// <returns></returns>
    public static List<Tile> FindPath(Tile startTile, Tile endTile ) {
        if (startTile == null || endTile == null) {
            return null;
        }

        openList = new List<Tile> { startTile };
        closedList = new List<Tile>();

        for (int x = 0; x < Sugarscape.sugarscape.GetLength(0); x++) {
            for (int y = 0; y < Sugarscape.sugarscape.GetLength(1); y++) {
                Sugarscape.sugarscape[x, y].gCost = 9999;
                Sugarscape.sugarscape[x, y].CalculateFCost();
                Sugarscape.sugarscape[x, y].previousTile = null;
            }
        }

        startTile.gCost = 0;
        startTile.hCost = CalculateDistanceCost(startTile, endTile);
        startTile.CalculateFCost();

        //While we still have nodes to search
        while (openList.Count > 0) {
            //The A* algorithm starts with the node with the lowest F cost, then evaluates its neighbours
            Tile currentNode = GetLowestFCostNode(openList);
            if (currentNode == endTile) {
                // Reached final node
                return CalculatePath(endTile);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Tile neighbourNode in GetNeighbourList(currentNode)) {
                //If this node has already been searched, ignore
                if (closedList.Contains(neighbourNode)) continue;
                //If this node is impassable, ignore
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                //The calculates a possible gCost for the node, based on the current node and the distance between it and the neighbour
                int possibleGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                //If it's the lowest gCost for the node..
                if (possibleGCost < neighbourNode.gCost) {
                    //Set this node as it's previous
                    neighbourNode.previousTile = currentNode;
                    //Update its g and h and f costs
                    neighbourNode.gCost = possibleGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endTile);
                    neighbourNode.CalculateFCost();
                    //Add it to the open list, if it's not already
                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    /// <summary>
    /// Fetches all valid neighbour nodes for the simulation
    /// </summary>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    private static List<Tile> GetNeighbourList(Tile currentNode) {
        List<Tile> neighbourList = new List<Tile>
        {
            currentNode.North,
            currentNode.East,
            currentNode.South,
            currentNode.West
        };

        return neighbourList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endTile"></param>
    /// <returns></returns>
    private static List<Tile> CalculatePath(Tile endTile) {
        List<Tile> path = new List<Tile> { endTile };
        Tile currentNode = endTile;
        while (currentNode.previousTile != null) {
            path.Add(currentNode.previousTile);
            currentNode = currentNode.previousTile;
        }
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Calculates the 'as the crow flies' distance between 2 nodes
    /// It does this by going diagonally as much as possible, and then vertically / horizontally
    /// </summary>
    /// <param name="a">The first node</param>
    /// <param name="b">The second node</param>
    /// <returns>The distance between the 2 nodes</returns>
    private static int CalculateDistanceCost(Tile a, Tile b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        //int remaining = Mathf.Abs(xDistance - yDistance);
        //return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        return xDistance * MOVE_STRAIGHT_COST + yDistance * MOVE_STRAIGHT_COST;
    }

    /// <summary>
    /// Searches the list of nodes and returns the node with the lowest F cost
    /// </summary>
    /// <param name="TileList">The list of nodes to search</param>
    /// <returns>The node with the lowest F cost</returns>
    private static Tile GetLowestFCostNode(List<Tile> TileList) {
        Tile lowestFCostNode = TileList[0];
        for (int i = 1; i < TileList.Count; i++) {
            if (TileList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = TileList[i];
            }
        }
        return lowestFCostNode;
    }

    /// <summary>
    /// Given a tile, returns the closest sugar containing tile
    /// </summary>
    /// <param name="startTile">The tile to start the search from</param>
    /// <returns>The closest tile containing sugar</returns>
    public static Tile FindClosestSugar(Tile startTile) {
        var sugarDistances = new List<KeyValuePair<int, Tile>>();
        foreach (Tile sugarTile in Sugarscape.sugarscape)
        {
            if (sugarTile.Sugar == 4) {
                sugarDistances.Add(new KeyValuePair<int, Tile>(CalculateDistanceCost(startTile, sugarTile), sugarTile));
            }
    
        }
        sugarDistances.Sort((x, y) => x.Key.CompareTo(y.Key));
        return sugarDistances[0].Value;
    }

}
