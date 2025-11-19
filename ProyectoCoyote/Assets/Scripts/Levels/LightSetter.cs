using UnityEngine;

public class LightSetter : MonoBehaviour
{
        [Header("Render Settings")]
        public Material skyboxMaterial;
    public Color ambientColor = Color.gray;
    public float ambientIntensity = 1.0f;

    void Awake()
    {
        if (skyboxMaterial != null)
            RenderSettings.skybox = skyboxMaterial;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambientColor;
        RenderSettings.ambientIntensity = ambientIntensity;
        RenderSettings.reflectionIntensity = 0f;




        // Actualizar el lighting si es necesario
        DynamicGI.UpdateEnvironment();
    }

}
