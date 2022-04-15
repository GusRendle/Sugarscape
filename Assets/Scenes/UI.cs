
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text generateBtn;
    public Text playBtn;

    public Text viewDesc;
    public Text growth;
    public Text speed;
    public Text step;

    public InputField maxWealthField;
    public InputField minWealthField;
    public InputField maxMetabolismField;
    public InputField minMetabolismField;
    public InputField numberField;
    public InputField maxVisionField;
    public InputField minVisionField;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        growth.text = Simulation.Parameters.SUGAR_GROWTH_RATE.ToString();
        speed.text = State.STEPS_PER_SECOND.ToString();
        step.text = "Step: " + Simulation.CURRENT_STEP.ToString();
        if ( State.DONE ) {
            playBtn.transform.parent.GetComponent<Button>().interactable = false;
            playBtn.text = "Done";
        }
    }

    public void ColorsDefaultToggled (bool value) {
        if ( value ) {
            State.COLORING_OPTION = State.ColoringOptions.DEFAULT;
            viewDesc.text = " ";
            Simulation.Render();
        }
    }

    public void ColorsMetabolismToggled (bool value) {
        if ( value ) {
            State.COLORING_OPTION = State.ColoringOptions.BY_METABOLISM;
            viewDesc.text = "low = blue medium = purple high = red";
            Simulation.Render();
        }
    }

    public void ColorsSexToggled (bool value) {
        if ( value ) {
            State.COLORING_OPTION = State.ColoringOptions.BY_SEX;
            viewDesc.text = " ";
            Simulation.Render();
        }
    }

    public void ColorsVisionToggled (bool value) {
        if ( value ) {
            State.COLORING_OPTION = State.ColoringOptions.BY_VISION;
            viewDesc.text = "low = red medium = purple high = blue";
            Simulation.Render();
        }
    }

    public void WealthMaxUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Max(Simulation.Parameters.Wealth.MIN, int.Parse(value));
            Simulation.Parameters.Wealth.MAX = num;
            maxWealthField.text = num.ToString();
        } else {
            int num = Simulation.Parameters.Wealth.MIN;
            Simulation.Parameters.Wealth.MAX = num;
            maxWealthField.text = num.ToString();
        }
    }

    public void WealthMinUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(value), 0, Simulation.Parameters.Wealth.MAX);
            Simulation.Parameters.Wealth.MIN = num;
            minWealthField.text = num.ToString();
        } else {
            Simulation.Parameters.Wealth.MIN = 0;
            minWealthField.text = "0";
        }
    }

    public void FasterButtonClicked () {
        State.STEPS_PER_SECOND = Mathf.Clamp(++State.STEPS_PER_SECOND, 1, 30);
    }

    public void GenerateButtonClicked () {
        generateBtn.text = "Generate";
        playBtn.transform.parent.GetComponent<Button>().interactable = true;
        playBtn.text = "Play";
        State.PAUSED = true;
        State.DONE = false;
        Simulation.Init();
        Simulation.Render();
    }

    public void LessButtonClicked () {
        Simulation.Parameters.SUGAR_GROWTH_RATE = Mathf.Clamp(--Simulation.Parameters.SUGAR_GROWTH_RATE, 1, 4);
    }

    public void MetabolismMaxUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Max(Simulation.Parameters.Metabolism.MIN, int.Parse(value));
            Simulation.Parameters.Metabolism.MAX = num;
            maxMetabolismField.text = num.ToString();
        } else {
            int num = Simulation.Parameters.Metabolism.MIN;
            Simulation.Parameters.Metabolism.MAX = num;
            maxMetabolismField.text = num.ToString();
        }
    }

    public void MetabolismMinUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(value), 0, Simulation.Parameters.Metabolism.MAX);
            Simulation.Parameters.Metabolism.MIN = num;
            minMetabolismField.text = num.ToString();
        } else {
            Simulation.Parameters.Metabolism.MIN = 0;
            minMetabolismField.text = "0";
        }
    }

    public void MoreButtonClicked () {
        Simulation.Parameters.SUGAR_GROWTH_RATE = Mathf.Clamp(++Simulation.Parameters.SUGAR_GROWTH_RATE, 1, 4);
    }

    public void MovementClassicToggled (bool value) {
        if ( value ) {
            Simulation.Parameters.MOVEMENT_STRATEGY = Simulation.MovementStrategies.CLASSIC;
        }
    }

    public void MovementCustomToggled (bool value) {
        if ( value ) {
            Simulation.Parameters.MOVEMENT_STRATEGY = Simulation.MovementStrategies.CUSTOM;
        }
    }

    public void NumberFieldChanged (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Min(int.Parse(value), 1000);
            numberField.text = num.ToString();
        }
    }

    public void NumberFieldUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(value), 0, 1000);
            Simulation.Parameters.INITIAL_NUMBER_OF_AGENTS = num;
            numberField.text = num.ToString();
        } else {
            Simulation.Parameters.INITIAL_NUMBER_OF_AGENTS = 0;
            numberField.text = "0";
        }
    }

    public void PlayButtonClicked () {
        generateBtn.text = "Reset";
        if ( State.PAUSED ) {
            State.PAUSED = false;
            playBtn.text = "Pause";
            State.DELTA_TIME = 0;
            Simulation.Step();
            Simulation.Render();
        } else {
            State.PAUSED = true;
            playBtn.text = "Play";
        }
    }

    public void SlowerButtonClicked () {
        State.STEPS_PER_SECOND = Mathf.Clamp(--State.STEPS_PER_SECOND, 1, 30);
    }

    public void VisionMaxUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(value), Simulation.Parameters.Vision.MIN, 50);
            Simulation.Parameters.Vision.MAX = num;
            maxVisionField.text = num.ToString();
        } else {
            int num = Simulation.Parameters.Vision.MIN;
            Simulation.Parameters.Vision.MAX = num;
            maxVisionField.text = num.ToString();
        }
    }

    public void VisionMinUnfocused (string value) {
        if ( value.Length > 0 ) {
            int num = Mathf.Clamp(int.Parse(value), 0, Simulation.Parameters.Vision.MAX);
            Simulation.Parameters.Vision.MIN = num;
            minVisionField.text = num.ToString();
        } else {
            Simulation.Parameters.Vision.MIN = 0;
            minVisionField.text = "0";
        }
    }

}
