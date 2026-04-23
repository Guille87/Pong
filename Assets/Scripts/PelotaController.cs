using System.Collections;
using UnityEngine;

public class PelotaController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip sfxPaddle;
    [SerializeField] private AudioClip sfxWall;
    [SerializeField] private AudioClip sfxFail;

    [Header("Física")]
    [SerializeField] private float fuerzaInicial = 10f;
    [SerializeField] private float multiplicadorVelocidad = 1.05f;
    [SerializeField] private float velocidadMaxima = 25f; // Límite para evitar errores de física
    [SerializeField] private float delayLanzamiento = 1f;

    Rigidbody2D rb;
    private Coroutine corrutinaLanzamiento;

    private const float MIN_ANG = 25f;
    private const float MAX_ANG = 40f;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    public void LanzarDesdeCentro(int direccionX)
    {
        // Si ya había una corrutina en marcha, la cancelamos para evitar duplicar velocidad
        if (corrutinaLanzamiento != null) StopCoroutine(corrutinaLanzamiento);
        
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        corrutinaLanzamiento = StartCoroutine(LanzarPelota(direccionX));
    }

    private IEnumerator LanzarPelota(int direccionX)
    {
        yield return new WaitForSeconds(delayLanzamiento);
        
        float angulo = Random.Range(MIN_ANG, MAX_ANG) * Mathf.Deg2Rad;
        int direccionY = Random.value > 0.5f ? 1 : -1;
        
        Vector2 direccionVector = new Vector2(Mathf.Cos(angulo) * direccionX, Mathf.Sin(angulo) * direccionY);
        
        rb.linearVelocity = direccionVector * fuerzaInicial;
        corrutinaLanzamiento = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pala")) // Tag genérico para todas las palas
        {
            AudioManager.Instance.PlaySound(sfxPaddle, 0.2f);
            IncrementarVelocidad();
        }
        else if (collision.gameObject.CompareTag("Circulos"))
        {
            AudioManager.Instance.PlaySound(sfxWall, 0.2f);
            CorregirAnguloVertical();
        }
        else
        {
            AudioManager.Instance.PlaySound(sfxWall, 0.2f);
        }
    }

    private void CorregirAnguloVertical()
    {
        Vector2 vel = rb.linearVelocity;
        
        // Si el movimiento horizontal (X) es muy pequeño comparado con el vertical (Y)
        // significa que la pelota está rebotando casi verticalmente.
        if (Mathf.Abs(vel.x) < 3f) 
        {
            // Le damos un impulso mínimo en X para que avance hacia un lado
            // Si vel.x es casi 0, usamos la posición para saber si mandarla a la derecha o izquierda
            float nuevaX = (vel.x >= 0) ? 5f : -5f;
            rb.linearVelocity = new Vector2(nuevaX, vel.y);
        }
    }

    private void IncrementarVelocidad()
    {
        Vector2 nuevaVelocidad = rb.linearVelocity * multiplicadorVelocidad;
        
        if (nuevaVelocidad.magnitude <= velocidadMaxima)
        {
            rb.linearVelocity = nuevaVelocidad;
        }
        else
        {
            rb.linearVelocity = nuevaVelocidad.normalized * velocidadMaxima;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Porteria"))
        {
            AudioManager.Instance.PlaySound(sfxFail, 0.3f);
        }
    }
}
