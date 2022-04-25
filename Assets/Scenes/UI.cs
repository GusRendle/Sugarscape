
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using UnityEditor;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public Text generateBtn;
    public Text runBtn;

    public Text viewDesc;
    public Text regrowthRate;
    public Text speed;
    public Text step;

    public Text statsStep;
    public Text statsAgent;
    public Text statsGreed;
    public Text statsIncome;
    public Text statsMetab;

    public InputField maxWealthField;
    public InputField minWealthField;
    public InputField maxMetabolismField;
    public InputField minMetabolismField;
    public InputField numberField;
    public InputField maxVisionField;
    public InputField minVisionField;
    public InputField maxAgentSpeedField;
    public InputField minAgentSpeedField;
    public InputField maxGreedField;
    public InputField minGreedField;

    //Posible views for the simulation
    public enum ViewOptions {
        DEFAULT,
        METABOLISM,
        SEX,
        VISION
    }

    //Current view
    public static ViewOptions currentView = ViewOptions.DEFAULT;

    private static List<StepStat> multiRunList = new List<StepStat>();

    // Update is called once per frame
    void Update() {
        //Update labels with new information
        regrowthRate.text = Simulation.growbackRate.ToString();
        speed.text = Main.stepsPerSecond.ToString();
        step.text = "Step: " + Simulation.CurrentStep.ToString();
        statsStep.text = Simulation.stepsStats;
        statsAgent.text = Simulation.agentCountStats;
        statsGreed.text = Simulation.greedStats;
        statsIncome.text = Simulation.incomeStats;
        statsMetab.text = Simulation.metabStats;
        //If the simulation is complete (all agents dead), alter run btn
        if ( Main.isComplete ) {
            runBtn.transform.parent.GetComponent<Button>().interactable = false;
            runBtn.text = "Done!";
        }
        //If a MultiRun step is complete
        if (Simulation.agentStats.endStepStat != null) {
            multiRunList.Add(Simulation.agentStats.startStepStat);
            multiRunList.Add(Simulation.agentStats.stableStepStat);
            multiRunList.Add(Simulation.agentStats.dropStepStat);
            multiRunList.Add(Simulation.agentStats.endStepStat);
            GenBtnClicked();
            MultiRunBtnClicked();
        }
    }

    /// <summary>
    /// Runs when the user clicks the generate button
    /// </summary>
    public void GenBtnClicked () {
        generateBtn.text = "Generate";
        runBtn.transform.parent.GetComponent<Button>().interactable = true;
        runBtn.text = "Play";
        Main.isPaused = true;
        Main.isComplete = false;
        GenerateCSV(Simulation.stepStatList, "");
        Simulation.Initialise();
    }

    public void GenerateCSV(List<StepStat> list, string filePrefix) {
        var sb = new StringBuilder("Step, Agent Count, Avg Wealth, Avg Income, Avg Vision, Avg Metab, Avg Speed, Avg Path, Avg Greed");
            foreach (var stepStat in list) {
                sb.Append('\n').Append(stepStat.Step.ToString()).Append(',').Append(stepStat.AgentCount.ToString()).Append(',')
                .Append(stepStat.AvgWealth.ToString()).Append(',').Append(stepStat.AvgIncome.ToString()).Append(',')
                .Append(stepStat.AvgVision.ToString()).Append(',').Append(stepStat.AvgMetab.ToString()).Append(',')
                .Append(stepStat.AvgSpeed.ToString()).Append(',').Append(stepStat.AvgPath.ToString()).Append(',')
                .Append(stepStat.AvgGreed.ToString());
            }

            var folder = Application.streamingAssetsPath;
            if(!Directory.Exists(folder)) { Directory.CreateDirectory(folder); };
            var filePath = Path.Combine(folder, filePrefix + DateTime.Now.ToString("HH mm ss") + ".csv");

            using var writer = new StreamWriter(filePath, false);
            writer.Write(sb.ToString());
            AssetDatabase.Refresh();
    }

    /// <summary>
    /// Runs when the user clicks the run button
    /// </summary>
    public void RunBtnClicked () {
        generateBtn.text = "Reset";
        if ( Main.isPaused ) {
            Main.isPaused = false;
            runBtn.text = "Pause";
            Main.cumDeltaTime = 0;
            Simulation.MultiRun = false;
            Simulation.Step();
            Simulation.Render();
        } else {
            Main.isPaused = true;
            runBtn.text = "Play";
        }
    }

    public void MultiRunBtnClicked () {
        generateBtn.text = "Reset";
        if ( Main.isPaused ) {
            Main.isPaused = false;
            runBtn.text = "Pause";
            Main.cumDeltaTime = 0;
            Simulation.MultiRun = true;
            Simulation.Step();
            Simulation.Render();
        } else {
            Main.isPaused = true;
            GenerateCSV(multiRunList, "MultiRun");
            multiRunList.Clear();
        }
    }

    /// <summary>
    /// Runs when the user clicks the increase growth button
    /// </summary>
    public void GrowthIncBtnClicked () {
        Simulation.growbackRate = Mathf.Clamp(++Simulation.growbackRate, 0, 4);
    }

    /// <summary>
    /// Runs when the user clicks the decrease growth button
    /// </summary>
    public void GrowthDecBtnClicked () {
        Simulation.growbackRate = Mathf.Clamp(--Simulation.growbackRate, 0, 4);
    }

    /// <summary>
    /// Runs when the user clicks the increase speed button
    /// </summary>
    public void SpeedIncBtnClicked () {
        Main.stepsPerSecond = Mathf.Clamp(++Main.stepsPerSecond, 1, 200);
    }

    /// <summary>
    /// Runs when the user clicks the decrease speed button
    /// </summary>
    public void SpeedDecBtnClicked () {
        Main.stepsPerSecond = Mathf.Clamp(--Main.stepsPerSecond, 1, 200);
    }

    /// <summary>
    /// Runs when the user clicks the decrease speed button
    /// </summary>
    public void RegrowthDropBtnClicked () {
        Simulation.growbackRate -= Simulation.growbackRate / 2;
    }

    /// <summary>
    /// Toggles the default view.
    /// </summary>
    /// <param name="isToggled">If the view has been toggled</param>
    public void DefaultViewToggled (bool isToggled) {
        if ( isToggled ) {
            currentView = ViewOptions.DEFAULT;
            viewDesc.text = " ";
            Simulation.Render();
        }
    }
    
    /// <summary>
    /// Toggles the Metabolism view, where agents are coloured based on metabolism.
    /// </summary>
    /// <param name="isToggled">If the view has been toggled</param>
    public void MetabolismViewToggled (bool isToggled) {
        if ( isToggled ) {
            currentView = ViewOptions.METABOLISM;
            viewDesc.text = "low = blue medium = purple high = red";
            Simulation.Render();
        }
    }

    /// <summary>
    /// Toggles the Sex view
    /// </summary>
    /// <param name="isToggled">If the view has been toggled</param>
    public void SexViewToggled (bool isToggled) {
        if ( isToggled ) {
            currentView = ViewOptions.SEX;
            viewDesc.text = " ";
            Simulation.Render();
        }
    }

    /// <summary>
    /// Toggles the Vision view, where agents are coloured based on sight
    /// </summary>
    /// <param name="isToggled">If the view has been toggled</param>
    public void VisionViewToggled (bool isToggled) {
        if ( isToggled ) {
            currentView = ViewOptions.VISION;
            viewDesc.text = "low = red medium = purple high = blue";
            Simulation.Render();
        }
    }
    
    /// <summary>
    /// Runs when the user enters a value for max wealth
    /// </summary>
    /// <param name="maxWealth">The new max wealth</param>
    public void MaxWealthUnfocused (string maxWealth) {
        if ( maxWealth.Length > 0 ) {
            int num = Mathf.Max(Simulation.Wealth.min, int.Parse(maxWealth));
            Simulation.Wealth.max = num;
            maxWealthField.text = num.ToString();
        } else {
            int num = Simulation.Wealth.min;
            Simulation.Wealth.max = num;
            maxWealthField.text = num.ToString();
        }
    }

    /// <summary>
    /// Runs when the user enters a value for min wealth
    /// </summary>
    /// <param name="minWealth">The new min wealth</param>
    public void MinWealthUnfocused (string minWealth) {
        if ( minWealth.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(minWealth), 0, Simulation.Wealth.max);
            Simulation.Wealth.min = num;
            minWealthField.text = num.ToString();
        } else {
            Simulation.Wealth.min = 0;
            minWealthField.text = "0";
        }
    }

    /// <summary>
    /// Runs when the user enters a value for max metabolism
    /// </summary>
    /// <param name="maxMetabolism">The max metabolism</param>
    public void MaxMetabolismUnfocused (string maxMetabolism) {
        if ( maxMetabolism.Length > 0 ) {
            int num = Mathf.Max(Simulation.Metabolism.min, int.Parse(maxMetabolism));
            Simulation.Metabolism.max = num;
            maxMetabolismField.text = num.ToString();
        } else {
            int num = Simulation.Metabolism.min;
            Simulation.Metabolism.max = num;
            maxMetabolismField.text = num.ToString();
        }
    }

    /// <summary>
    /// Runs when the user enters a value for min metabolism
    /// </summary>
    /// <param name="minMetabolism">The min metabolism</param>
    public void MinMetabolismUnfocused (string minMetabolism) {
        if ( minMetabolism.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(minMetabolism), 0, Simulation.Metabolism.max);
            Simulation.Metabolism.min = num;
            minMetabolismField.text = num.ToString();
        } else {
            Simulation.Metabolism.min = 0;
            minMetabolismField.text = "0";
        }
    }
    /// <summary>
    /// Runs when the user clicks then Classic Movement checkbox
    /// </summary>
    /// <param name="isToggled">If the checkbox is toggled</param>
    public void ClassicMovementToggled (bool isToggled) {
        if ( isToggled ) {
            Simulation.movementStyle = Simulation.MovementStyle.CLASSIC;
        }
    }

    /// <summary>
    /// Runs when the user clicks then Custom Movement checkbox
    /// </summary>
    /// <param name="isToggled">If the checkbox is toggled</param>

    public void CustomMovementToggled (bool isToggled) {
        if ( isToggled ) {
            Simulation.movementStyle = Simulation.MovementStyle.CUSTOM;
            Simulation.Vision.max = 49;
            maxVisionField.text = "49";
            Simulation.Vision.min = 49;
            minVisionField.text ="49";
            Simulation.Wealth.max = 6;
            maxWealthField.text = "6";
            Simulation.Wealth.min = 1;
            minWealthField.text ="1";
            Simulation.Wealth.max = 6;
            maxWealthField.text = "6";
            Simulation.Wealth.min = 1;
            minWealthField.text ="1";

            Simulation.growbackRate = 49;

        }
    }

    /// <summary>
    /// Runs when the user enters a value for the amount of agents
    /// </summary>
    /// <param name="agentCount">The value in the input field</param>
    public void AgentCountUnfocused (string agentCount) {
        if ( agentCount.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(agentCount), 0, 1000);
            Simulation.initialAgentCount = num;
            numberField.text = num.ToString();
        } else {
            Simulation.initialAgentCount = 0;
            numberField.text = "0";
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the max vision of agents
    /// </summary>
    /// <param name="maxVision">The value in the input field</param>
    public void MaxVisionUnfocused (string maxVision) {
        if ( maxVision.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(maxVision), Simulation.Vision.min, 50);
            Simulation.Vision.max = num;
            maxVisionField.text = num.ToString();
        } else {
            int num = Simulation.Vision.min;
            Simulation.Vision.max = num;
            maxVisionField.text = num.ToString();
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the min vision of agents
    /// </summary>
    /// <param name="minVision">The value in the input field</param>
    public void MinVisionUnfocused (string minVision) {
        if ( minVision.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(minVision), 0, Simulation.Vision.max);
            Simulation.Vision.min = num;
            minVisionField.text = num.ToString();
        } else {
            Simulation.Vision.min = 0;
            minVisionField.text = "0";
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the max speed of agents
    /// </summary>
    /// <param name="maxAgentSpeed">The value in the input field</param>
    public void MaxAgentSpeedUnfocused (string maxAgentSpeed) {
        if ( maxAgentSpeed.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(maxAgentSpeed), Simulation.AgentSpeed.min, 10);
            Simulation.AgentSpeed.max = num;
            maxAgentSpeedField.text = num.ToString();
        } else {
            int num = Simulation.AgentSpeed.min;
            Simulation.AgentSpeed.max = num;
            maxAgentSpeedField.text = num.ToString();
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the min speed of agents
    /// </summary>
    /// <param name="minAgentSpeed">The value in the input field</param>
    public void MinAgentSpeedUnfocused (string minAgentSpeed) {
        if ( minAgentSpeed.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(minAgentSpeed), 0, Simulation.AgentSpeed.max);
            Simulation.AgentSpeed.min = num;
            minAgentSpeedField.text = num.ToString();
        } else {
            Simulation.AgentSpeed.min = 0;
            minAgentSpeedField.text = "0";
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the max speed of agents
    /// </summary>
    /// <param name="maxGreed">The value in the input field</param>
    public void MaxGreedUnfocused (string maxGreed) {
        if ( maxGreed.Length > 0 ) {
            float num = Mathf.Clamp(float.Parse(maxGreed), Simulation.Greed.min, 2f);
            Simulation.Greed.max = num;
            maxGreedField.text = num.ToString();
        } else {
            float num = Simulation.Greed.min;
            Simulation.Greed.max = num;
            maxGreedField.text = num.ToString();
        }
    }

    /// <summary>
    /// Runs when the user enters a value for the min speed of agents
    /// </summary>
    /// <param name="minGreed">The value in the input field</param>
    public void MinGreedUnfocused (string minGreed) {
        if ( minGreed.Length > 0 ) {
            float num = Mathf.Clamp(float.Parse(minGreed), 0f, Simulation.Greed.max);
            Simulation.Greed.min = num;
            minGreedField.text = num.ToString();
        } else {
            Simulation.Greed.min = 0;
            minGreedField.text = "0";
        }
    }

}