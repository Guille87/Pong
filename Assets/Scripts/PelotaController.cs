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
    private CameraShake camShake;

    private const float MIN_ANG = 25f;
    private const float MAX_ANG = 40f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (Camera.main != null)
        {
            camShake = Camera.main.GetComponent<CameraShake>();
        }
    }

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
            CorregirAnguloVertical();
        }
        else if (collision.gameObject.CompareTag("Circulos"))
        {
            AudioManager.Instance.PlaySound(sfxWall, 0.2f);
            CorregirAnguloVertical();
            // Sacudida
            if (camShake != null) camShake.Shake(0.12f, 0.07f);
        }
        else
        {
            AudioManager.Instance.PlaySound(sfxWall, 0.2f);
        }
    }

    private void CorregirAnguloVertical()
    {
        Vector2 vel = rb.linearVelocity;
        float magnitudActual = vel.magnitude;

        bool necesitaAjuste = false;
        
        // Si el movimiento horizontal (X) es muy pequeño comparado con el vertical (Y)
        // significa que la pelota está rebotando casi verticalmente.
        if (Mathf.Abs(vel.x) < 3f) 
        {
            // Le damos un impulso mínimo en X para que avance hacia un lado
            // Si vel.x es casi 0, usamos la posición para saber si mandarla a la derecha o izquierda
            float direccionX = (vel.x >= 0) ? 5f : -5f;
            vel.x = direccionX;
            necesitaAjuste = true;
        }

        if (Mathf.Abs(vel.y) < 0.5f)
        {
            // Le damos un pequeño empujón hacia arriba o abajo aleatoriamente
            float nuevaY = (Random.value > 0.5f) ? 1.5f : -1.5f;
            vel.y = nuevaY;
            necesitaAjuste = true;
        }

        if (necesitaAjuste)
        {
            // .normalized mantiene la dirección corregida, 
            // y al multiplicar por magnitudActual recuperamos la velocidad que tenía.
            rb.linearVelocity = vel.normalized * magnitudActual;
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
            // Sacudida fuerte al marcar gol
            if (camShake != null) camShake.Shake(0.2f, 0.2f);
        }
    }
}
