using UnityEngine;
using Unity.Cinemachine;

public class CameraCollider : MonoBehaviour
{
    public CinemachineCamera cam;
    public Transform player;
    public Material material;
    public string transparencyPropertyName = "hitTransparency";
    public float fadeDistance = 1.5f;

    [Range(0f, 1f)]
    public float maxTransparency = 0.7f;

    private Transform camTransform;

    void LateUpdate()
    {
        if (cam == null || player == null || material == null)
            return;

        if (camTransform == null)
            camTransform = cam.transform;

        float distance = Vector3.Distance(camTransform.position, player.position);

        // Mapear distancia -> transparencia (start: transparencia 0, cerca: transparencia alta)
        float t = Mathf.Clamp01((fadeDistance - distance) / fadeDistance);
        float transparency = Mathf.Lerp(0f, maxTransparency, t);

        // Si existe la propiedad de transparencia, escribir ahí
        if (!string.IsNullOrEmpty(transparencyPropertyName) && material.HasProperty(transparencyPropertyName))
        {
            material.SetFloat(transparencyPropertyName, transparency);
            return;
        }

        // Fallback: si no hay propiedad dedicada, intentar _BaseColor o _Color (solo funcionará si el Graph usa su alpha)
        float alpha = 1f - transparency;
        if (material.HasProperty("_BaseColor"))
        {
            var c = material.GetColor("_BaseColor");
            c.a = alpha;
            material.SetColor("_BaseColor", c);
        }
        else if (material.HasProperty("_Color"))
        {
            var c = material.GetColor("_Color");
            c.a = alpha;
            material.SetColor("_Color", c);
        }
        // Si ninguna de estas afecta, el Graph no está leyendo ese alpha: usa una propiedad de transparencia del Graph.
    }
}
