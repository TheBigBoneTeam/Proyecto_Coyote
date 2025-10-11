using System.Collections;
using UnityEngine;
// Clase que se utiliza para colocar el indicador de enemigo lockeado
// de la UI siempre delante de la cámara

public class SimpleLockOn : MonoBehaviour
{
    
    [SerializeField] Transform camara;

    private void OnEnable()
    {
        if (camara == null) camara = Camera.main.transform;
        StartCoroutine(LookAtTarget());

    }
    // Cambiar posición y rotación en función de la cámara
    private IEnumerator LookAtTarget() 
    {
        while (this.gameObject.activeInHierarchy) 
        { 
            Vector3 _dir = camara.position - transform.position;
            transform.rotation = Quaternion.LookRotation(_dir);
            yield return null;
        }
    }
}
