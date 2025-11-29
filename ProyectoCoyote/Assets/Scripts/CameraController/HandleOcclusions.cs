using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class HandleOcclusions : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    Transform player;
    Transform playerObj;

    [Header("Trasparencias")]
    private List<MeshRenderer> disabledRenderers = new List<MeshRenderer>();
    // Shader
    [SerializeField] float occlusionAngle = 30f;
    public Material transparentMaterial;
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private Dictionary<Renderer, Material[]> instancedMaterials = new Dictionary<Renderer, Material[]>();
    private HashSet<Renderer> currentHits = new HashSet<Renderer>();
    private Renderer[] cachedRenderers;
    private HashSet<Renderer> initiallyActiveRenderers;
    
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player/Player_02").transform;
        cachedRenderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        initiallyActiveRenderers = new HashSet<Renderer>();
        foreach (Renderer rend in cachedRenderers)
        {
            if (rend.enabled)
            {
                initiallyActiveRenderers.Add(rend);
            }
        }
    }

    #region Gestión de Transparencias

    // Desactivando renderers
    public void HandleOcclusion()
    {
        Vector3 origin = cam.transform.position;
        Vector3 target = playerObj.position + Vector3.up * 1.5f;
        Vector3 dir = (target - origin).normalized;
        float dist = Vector3.Distance(origin, target);

        // Restaurar renderers
        foreach (MeshRenderer rend in disabledRenderers)
        {
            if (rend != null) rend.enabled = true;
        }
        disabledRenderers.Clear();

        // Desactivar renderers
        foreach (MeshRenderer rend in cachedRenderers)
        {
            if (!initiallyActiveRenderers.Contains(rend))
                continue;

            if (DontApplyToObject(rend))
                continue;

            
            if (IsRendererObstructingView(rend, origin, target, dist))
            {
                rend.enabled = false;
                disabledRenderers.Add(rend);
            }
        }
    }

    // Usando material específico
    public void HandleTransparency()
    {
        Vector3 origin = cam.transform.position;
        Vector3 target = playerObj.position + Vector3.up * 1.5f;
        float dist = Vector3.Distance(origin, target);

        HashSet<Renderer> newHits = new HashSet<Renderer>();

        
        foreach (Renderer rend in cachedRenderers)
        {
            if (!initiallyActiveRenderers.Contains(rend))
                continue;
            if(DontApplyToObject(rend))
                continue;
            

            if (IsRendererObstructingView(rend, origin, target, dist))
            {
                newHits.Add(rend);

                // Guardar materiales originales si no los tenemos
                if (!originalMaterials.ContainsKey(rend))
                {
                    originalMaterials[rend] = rend.sharedMaterials;
                }

                // Crear o reutilizar material transparente
                if (!instancedMaterials.ContainsKey(rend))
                {
                    Material[] originalMats = originalMaterials[rend];
                    Material[] transparentMats = new Material[originalMats.Length];

                    for (int i = 0; i < originalMats.Length; i++)
                    {
                        Material transparentInstance = new Material(transparentMaterial);

                        // Intentar copiar la textura principal del material original
                        Texture mainTex = null;

                        if (originalMats[i].HasProperty("_Texture2D"))
                        {
                            mainTex = originalMats[i].GetTexture("_Texture2D");
                        }
                        else
                        {
                            // Si no tiene "_Texture2D", usar su textura principal
                            mainTex = originalMats[i].mainTexture;
                        }

                        // Aplicar la textura al material transparente si existe
                        if (mainTex != null)
                        {
                            if (transparentInstance.HasProperty("_Texture2D"))
                            {
                                transparentInstance.SetTexture("_Texture2D", mainTex);
                            }
                            else
                            {
                                transparentInstance.mainTexture = mainTex;
                            }
                        }

                        transparentMats[i] = transparentInstance;
                    }

                    instancedMaterials[rend] = transparentMats;
                }

                // Aplicar materiales transparentes
                rend.sharedMaterials = instancedMaterials[rend];
            }
        }


        // Restaurar renderers que ya no están obstruyendo
        foreach (Renderer rend in currentHits)
        {
            if (rend != null && !newHits.Contains(rend))
            {
                if (originalMaterials.ContainsKey(rend))
                {
                    rend.sharedMaterials = originalMaterials[rend];
                }
            }
        }

        currentHits = newHits;
    }
    private void OnDestroy()
    {
        // Limpiar materiales instanciados
        foreach (var kvp in instancedMaterials)
        {
            if (kvp.Value != null)
            {
                foreach (Material mat in kvp.Value)
                {
                    if (mat != null)
                    {
                        Destroy(mat);
                    }
                }
            }
        }

        // Restaurar todos los materiales originales
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
            {
                kvp.Key.sharedMaterials = kvp.Value;
            }
        }
    }
    #endregion

    #region Comprobaciones
    private bool DontApplyToObject(Renderer rend) 
    {
        // Layers a ignorar
        int groundLayer = LayerMask.NameToLayer("whatIsGround");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (rend.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
            return true;
        
        if (IsInLayer(rend.transform, enemyLayer))
            return true;
        
        if (rend.transform == playerObj || rend.transform.IsChildOf(playerObj))
            return true;

        if (player != null && (rend.transform == player || rend.transform.IsChildOf(player)))
            return true;

        return false;
    }
    private bool IsInLayer(Transform trans, int layer)
    {
        
        if (trans.gameObject.layer == layer)
            return true;

        // Verificar padres
        Transform current = trans.parent;
        while (current != null)
        {
            if (current.gameObject.layer == layer)
                return true;
            current = current.parent;
        }

        return false;
    }
    #endregion

    #region Cálculo de objetos obstruyendo
    private bool IsRendererObstructingView(Renderer rend, Vector3 cameraPos, Vector3 playerPos, float maxDist)
    {
        Bounds bounds = rend.bounds;

        // Dirección desde el jugador a la cámara
        Vector3 dirToCamera = (cameraPos - playerPos).normalized;

        // Obtener el punto más cercano del objeto al jugador
        Vector3 closestPointToPlayer = bounds.ClosestPoint(playerPos);
        float distanceAlongLine = Vector3.Dot(closestPointToPlayer - playerPos, dirToCamera);

        // Si está detrás del jugador o más allá de la cámara, no obstruye
        if (distanceAlongLine < -0.5f || distanceAlongLine > maxDist)
            return false;

        // Verificar si el objeto es visible desde la cámara
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (!GeometryUtility.TestPlanesAABB(frustumPlanes, bounds))
            return false;

        // Verificar múltiples puntos del objeto
        Vector3[] testPoints = GetTestPoints(bounds);
        float angleThreshold = occlusionAngle;

        foreach (Vector3 point in testPoints)
        {
            // Distancia desde el jugador al punto
            float distToPoint = Vector3.Distance(playerPos, point);

            // Solo considerar puntos entre jugador y cámara
            if (distToPoint > -0.5f && distToPoint < maxDist)
            {
                // Dirección desde el jugador al punto
                Vector3 dirToPoint = (point - playerPos).normalized;
                float angle = Vector3.Angle(dirToCamera, dirToPoint);

                if (angle < angleThreshold)
                {
                    return true;
                }
            }
        }

        // Comprobar si el objeto interseca con el volumen entre jugador y cámara
        return ViewCone(bounds, playerPos, cameraPos, angleThreshold);
    }

    private bool ViewCone(Bounds bounds, Vector3 playerPos, Vector3 cameraPos, float angleThreshold)
    {
        //Cono desde el jugador hacia la cámara
        Vector3 dirToCamera = (cameraPos - playerPos).normalized;
        Vector3 closestPointOnLine = playerPos + dirToCamera * Vector3.Dot(bounds.center - playerPos, dirToCamera);
        Vector3 closestPointOnBounds = bounds.ClosestPoint(closestPointOnLine);

        // Verificar si ese punto está dentro del cono
        float distToPlayer = Vector3.Distance(playerPos, closestPointOnBounds);
        float distToCamera = Vector3.Distance(playerPos, cameraPos);

        // Debe estar entre jugador y cámara
        if (distToPlayer < -0.5f || distToPlayer > distToCamera)
            return false;

        // Radio del cono en ese punto (vértice en el jugador)
        float coneRadius = distToPlayer * Mathf.Tan(angleThreshold * Mathf.Deg2Rad);
        float distanceFromAxis = Vector3.Distance(closestPointOnBounds, playerPos + dirToCamera * distToPlayer);

        return distanceFromAxis < coneRadius;
    }


    private Vector3[] GetTestPoints(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        // Incluir para mejor detección
        return new Vector3[]
        {
            // Esquinas
            center + new Vector3(extents.x, extents.y, extents.z),
            center + new Vector3(extents.x, extents.y, -extents.z),
            center + new Vector3(extents.x, -extents.y, extents.z),
            center + new Vector3(extents.x, -extents.y, -extents.z),
            center + new Vector3(-extents.x, extents.y, extents.z),
            center + new Vector3(-extents.x, extents.y, -extents.z),
            center + new Vector3(-extents.x, -extents.y, extents.z),
            center + new Vector3(-extents.x, -extents.y, -extents.z),
            // Centros de caras
            center + new Vector3(extents.x, 0, 0),
            center + new Vector3(-extents.x, 0, 0),
            center + new Vector3(0, extents.y, 0),
            center + new Vector3(0, -extents.y, 0),
            center + new Vector3(0, 0, extents.z),
            center + new Vector3(0, 0, -extents.z),
            // Centro del bounds
            center
        };
    }



    #endregion

    //private void HandleTransparency()
    //{
    //    Vector3 origin = cam.transform.position;
    //    Vector3 target = playerObj.position + Vector3.up * 1.5f;
    //    Vector3 dir = target - origin;
    //    float dist = dir.magnitude;

    //    // Restaurar materiales
    //    foreach (MeshRenderer rend in disabledRenderers)
    //    {
    //        if (rend != null && originalMaterials.ContainsKey(rend))
    //        {
    //            rend.material = originalMaterials[rend];
    //        }
    //    }
    //    currentHits.Clear();

    //    foreach (MeshRenderer rend in cachedRenderers)
    //    {
    //        if (!initiallyActiveRenderers.Contains(rend))
    //            continue;

    //        if (rend.transform.IsChildOf(playerObj) || rend.transform == playerObj || rend.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
    //            continue;


    //        var texture = rend.material.GetTexture("_Texture2D");
    //        if (IsRendererObstructingView(rend, origin, target, dist))
    //        {
    //            // Guardar material original si no lo tenemos
    //            if (!originalMaterials.ContainsKey(rend))
    //            {
    //                originalMaterials[rend] = rend.material;
    //            }

    //            // Aplicar material transparente
    //            rend.material = transparentMaterial;
    //            rend.material.SetTexture("_Texture2D", texture);
    //            // Añadir a lista de objetos transparentes este frame
    //            currentHits.Add(rend);
    //        }
    //    }


    //}
}
