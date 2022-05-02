
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
        }
        Simulation.Render();
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

    public static float GaussianRandom(float min, float max) {
        float median = (max - min) / 2;
        float stdDev = (median - min) / 3;
        float rnd = max + 1;
        while (rnd > max || rnd < min)
        {
            float i1 = Random.Range(0.0f, 1.0f);
            float i2 = Random.Range(0.0f, 1.0f);
            rnd = median + stdDev * Mathf.Sqrt(-2.0f * Mathf.Log(i1)) * Mathf.Sin(i2 * 2f * Mathf.PI );
        }
        return  rnd;
    }

    public static int IntDistrobutionRandom(float one, float two, float three, float four, float five, float six) {
        float max = one + two + three + four + five + six;
        float rnd = Random.Range(0f, max);
        if (rnd <= one ) {
            return 1;
        } else if (rnd <= two + one ) {
            return 2;
        } else if (rnd <= three + two + one ) {
            return 3;
        } else if (rnd <= four + three + two + one) {
            return 4;
        } else if (rnd <= five + four + three + two + one) {
            return 5;
        } else if (rnd <= max) {
            return 6;
        } else {
            return 999;
        }
    }

}


