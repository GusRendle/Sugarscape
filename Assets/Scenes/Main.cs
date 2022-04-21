
using UnityEngine;

using System.Collections.Generic;

public class Main : MonoBehaviour {

    //The delta time of this state
    public static float cumDeltaTime = 0;
    //Is the game paused?
    public static bool isPaused = true;
    //Is the simulation over? (all agents dead)
    public static bool isComplete = false;
    //Manages speed of the simulation
    public static int stepsPerSecond = 4;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start() {
        //Generates a sugarscape with default values
        Simulation.Initialise();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update() {
        if ( isPaused || isComplete ) {
            return;
        }
        // Seconds since last frame are added to cumulative delta time 
        cumDeltaTime += Time.deltaTime;
        float secondsPerStep = 1.0f / stepsPerSecond;
        //The number of steps to run this frame are calculated
        for ( int i = (int) ( cumDeltaTime / secondsPerStep ) ; i > 0 ; i-- ) {
            cumDeltaTime -= secondsPerStep;
            //Moves the simulation forward a step, if all agents are dead, stops
            if ( !Simulation.Step() ) {
                isComplete = true;
            }
            Simulation.Render();
        }
    }

    /// <summary>
    /// Implements the Fisher-Yates shuffle to efficiently randomly shuffle the inputted list
    /// </summary>
    /// <param name="list">The list to shuffle</param>
    /// <typeparam name="T">Type in list</typeparam>
    /// <returns>The shuffled list</returns>
    public static List<T> Shuffle<T> (List<T> list) {
        List<T> shuffledList = new List<T>(list);
        var rand = new System.Random();
        int n = shuffledList.Count;  
        while (n > 1) {  
            n--;  
            int i = rand.Next(n + 1);
            (shuffledList[i], shuffledList[n]) = (shuffledList[n], shuffledList[i]);
        } 
        return shuffledList;
    }

}


