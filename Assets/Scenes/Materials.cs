
using UnityEngine;

public class Materials {

    //Shader for all materials
    private static readonly Shader diffuseShader = Shader.Find("Legacy Shaders/Diffuse");

    //Materials for the sugarscape
    public static readonly Material Background = new Material(diffuseShader) { color = Color.white };
    public static readonly Material Sugar = new Material(diffuseShader) { color = Color.yellow };
    public static readonly Material DefaultColour = new Material(diffuseShader) { color = Color.red };

    //Materials for the custom views
    public static readonly Material LowColour = new Material(diffuseShader) { color = Color.blue };
    public static readonly Material MedColour = new Material(diffuseShader) { color = new Color(0.5f, 0, 0.5f, 1) };
    public static readonly Material HighColour = new Material(diffuseShader) { color = Color.red };

}
