using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 posicionOriginal;

    void Awake()
    {
        posicionOriginal = transform.localPosition;
    }

    public void Shake(float duracion, float intensidad)
    {
        StopAllCoroutines();
        StartCoroutine(ProcesoShake(duracion, intensidad));
    }

    private IEnumerator ProcesoShake(float duracion, float intensidad)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            // Generamos un desplazamiento aleatorio
            float x = Random.Range(-1f, 1f) * intensidad;
            float y = Random.Range(-1f, 1f) * intensidad;

            transform.localPosition = new Vector3(x, y, posicionOriginal.z);

            tiempoTranscurrido += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        transform.localPosition = posicionOriginal;
    }
}
