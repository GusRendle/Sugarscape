
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text generateBtn;
    public Text runBtn;

    public Text viewDesc;
    public Text regrowthRate;
    public Text speed;
    public Text step;

    public Text statsStep;
    public Text statsAgent;
    public Text statsWealth;
    public Text statsIncome;
    public Text statsMetab;

    public InputField maxWealthField;
    public InputField minWealthField;
    public InputField maxMetabolismField;
    public InputField minMetabolismField;
    public InputField numberField;
    public InputField maxVisionField;
    public InputField minVisionField;

    //Posible views for the simulation
    public enum ViewOptions {
        DEFAULT,
        METABOLISM,
        SEX,
        VISION
    }

    //Current view
    public static ViewOptions currentView = ViewOptions.DEFAULT;

    // Update is called once per frame
    void Update() {
        //Update labels with new information
        regrowthRate.text = Simulation.growbackRate.ToString();
        speed.text = Main.stepsPerSecond.ToString();
        step.text = "Step: " + Simulation.CurrentStep.ToString();
        statsStep.text = Simulation.stepsStats;
        statsAgent.text = Simulation.agentStats;
        statsWealth.text = Simulation.wealthStats;
        statsIncome.text = Simulation.incomeStats;
        statsMetab.text = Simulation.metabStats;
        //If the simulation is complete (all agents dead), alter run btn
        if ( Main.isComplete ) {
            runBtn.transform.parent.GetComponent<Button>().interactable = false;
            runBtn.text = "Done!";
        }
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
        Main.stepsPerSecond = Mathf.Clamp(++Main.stepsPerSecond, 1, 30);
    }

    /// <summary>
    /// Runs when the user clicks the decrease speed button
    /// </summary>
    public void SpeedDecBtnClicked () {
        Main.stepsPerSecond = Mathf.Clamp(--Main.stepsPerSecond, 1, 30);
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
        Simulation.Initialise();
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
            Simulation.Step();
            Simulation.Render();
        } else {
            Main.isPaused = true;
            runBtn.text = "Play";
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

        }
    }

    /// <summary>
    /// Runs when the user enters a value for the amount of agents
    /// </summary>
    /// <param name="agentCount"></param>
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
    /// <param name="maxVision"></param>
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
    /// <param name="minVision"></param>
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

}
