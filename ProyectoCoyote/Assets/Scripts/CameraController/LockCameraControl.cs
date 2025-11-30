using Unity.Cinemachine;
using UnityEngine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("Cinemachine/Custom/Lock-On Player Centered Clamp")]
public class LockCameraControl : CinemachineExtension
{
    [Tooltip("Ángulo máximo a cada lado de la línea jugador->enemigo")]
    public float halfArc = 60f;

    [Tooltip("Suavizado al aplicar los límites")]
    public float dampingSpeed = 10f;

    [Tooltip("Offset vertical desde el jugador para calcular el centro")]
    public float verticalOffset = 1.5f;

    private float targetAngle;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body) return;

        // Obtener el Orbital Transposer
        var orbital = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineOrbitalFollow;
        if (orbital == null) return;

        Transform player = vcam.Follow;
        Transform enemy = vcam.LookAt;

        if (player == null || enemy == null) return;

        // Calcular la dirección jugador->enemigo en el plano XZ
        Vector3 playerPos = player.position;
        playerPos.y += verticalOffset; // Ajustar altura

        Vector3 toEnemy = enemy.position - playerPos;
        toEnemy.y = 0f;

        if (toEnemy.sqrMagnitude < 0.0001f) return;

        // Calcular el ángulo central (dirección jugador->enemigo)
        float centerAngle = Mathf.Atan2(toEnemy.x, toEnemy.z) * Mathf.Rad2Deg;

        // Obtener el ángulo actual del orbital
        float currentAngle = orbital.HorizontalAxis.Value;

        // Calcular el offset relativo al centro (jugador->enemigo)
        float angleOffset = Mathf.DeltaAngle(centerAngle, currentAngle);

        // Clampear el offset dentro del rango permitido
        float clampedOffset = Mathf.Clamp(angleOffset, -halfArc, halfArc);

        // Calcular el nuevo ángulo absoluto
        float targetAngle = centerAngle + clampedOffset;

        // Aplicar el nuevo valor
        orbital.HorizontalAxis.Value = targetAngle;

        // Actualizar el centro del eje para que coincida con la dirección jugador->enemigo
        orbital.HorizontalAxis.Center = centerAngle;
    }

    // Normalizar ángulo a rango -180 a 180
    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    // Clampear ángulo al rango, manejando wrap-around
    private float ClampAngleToRange(float angle, float min, float max, float center)
    {
        // Calcular distancia angular desde el centro
        float deltaFromCenter = Mathf.DeltaAngle(center, angle);

        // Si está dentro del rango, no hacer nada
        if (Mathf.Abs(deltaFromCenter) <= halfArc)
            return angle;

        // Si está fuera, clampear al borde más cercano
        if (deltaFromCenter > 0)
            return center + halfArc;
        else
            return center - halfArc;
    }

    // Visualización en el editor (útil para debugging)
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        var vcam = GetComponent<CinemachineCamera>();
        if (vcam == null || vcam.Follow == null || vcam.LookAt == null) return;

        Vector3 playerPos = vcam.Follow.position;
        playerPos.y += verticalOffset;
        Vector3 enemyPos = vcam.LookAt.position;

        // Línea jugador->enemigo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerPos, enemyPos);

        // Arco de movimiento permitido
        Vector3 toEnemy = enemyPos - playerPos;
        toEnemy.y = 0;
        if (toEnemy.sqrMagnitude > 0.0001f)
        {
            float distance = toEnemy.magnitude;
            float centerAngle = Mathf.Atan2(toEnemy.x, toEnemy.z) * Mathf.Rad2Deg;

            Gizmos.color = Color.yellow;
            DrawArc(playerPos, distance, centerAngle - halfArc, centerAngle + halfArc, 20);
        }
    }

    private void DrawArc(Vector3 center, float radius, float startAngle, float endAngle, int segments)
    {
        Vector3 prevPoint = center + Quaternion.Euler(0, startAngle, 0) * Vector3.forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, i / (float)segments);
            Vector3 point = center + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}