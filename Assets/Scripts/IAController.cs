using UnityEngine;

public class IAController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float margenError = 0.2f;

    private Transform pelota;
    private const float MAX_Y = 4.3f;
    private const float MIN_Y = -4.3f;

    void Update()
    {
        if (pelota == null)
        {
            GameObject objetoPelota = GameObject.FindGameObjectWithTag("Pelota");
            if (objetoPelota != null)
            {
                pelota = objetoPelota.transform;
            }
            else
            {
                return; // Si sigue sin existir/estar activa, salimos del Update este frame
            }
        }

        // Calculamos la diferencia de altura
        float diferenciaY = pelota.position.y - transform.position.y;

        // Solo se mueve si la pelota está fuera del margen de error (evita vibraciones)
        if (Mathf.Abs(diferenciaY) > margenError)
        {
            float direccion = diferenciaY > 0 ? 1 : -1;
            float nuevaY = transform.position.y + (direccion * velocidad * Time.deltaTime);
            
            nuevaY = Mathf.Clamp(nuevaY, MIN_Y, MAX_Y);
            transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
        }
    }
}
