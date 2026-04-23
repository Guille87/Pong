using UnityEngine;

public class IAController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float margenError = 0.2f;

    private Transform pelota;
    private const float MAX_Y = 4.3f;
    private const float MIN_Y = -4.3f;

    private bool esEquipoIzquierdo;

    void Start()
    {
        // Determinamos el equipo basándonos en su posición inicial en X
        esEquipoIzquierdo = transform.position.x < 0;
    }

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

        float objetivoY = 0; // Por defecto, el objetivo es el centro

        if (PelotaEstaDelante())
        {
            // Si la pelota está delante, la perseguimos
            objetivoY = pelota.position.y;
        }
        else
        {
            // Si la pelota está detrás, el objetivo se queda en 0 (el centro)
            objetivoY = 0;
        }

        MoverHacia(objetivoY);
    }

    private bool PelotaEstaDelante()
    {
        if (esEquipoIzquierdo)
        {
            // Para el equipo izquierdo, "delante" es que la pelota tenga una X mayor que la pala
            return pelota.position.x > transform.position.x;
        }
        else
        {
            // Para el equipo derecho, "delante" es que la pelota tenga una X menor que la pala
            return pelota.position.x < transform.position.x;
        }
    }

    private void MoverHacia(float objetivoY)
    {
        float diferenciaY = objetivoY - transform.position.y;

        if (Mathf.Abs(diferenciaY) > margenError)
        {
            float direccion = diferenciaY > 0 ? 1 : -1;
            float nuevaY = transform.position.y + (direccion * velocidad * Time.deltaTime);
            
            nuevaY = Mathf.Clamp(nuevaY, MIN_Y, MAX_Y);
            transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
        }
    }
}
