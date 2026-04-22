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

    private const float MIN_ANG = 25f;
    private const float MAX_ANG = 40f;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable() => ResetPelota(Random.Range(0, 2) == 0 ? -1 : 1);

    public void ResetPelota(int direccionX)
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        StartCoroutine(LanzarPelota(direccionX));
    }

    public void LanzarDesdeCentro(int direccionX) => ResetPelota(direccionX);

    private IEnumerator LanzarPelota(int direccionX)
    {
        yield return new WaitForSeconds(delayLanzamiento);
        
        float angulo = Random.Range(MIN_ANG, MAX_ANG) * Mathf.Deg2Rad;
        int direccionY = Random.value > 0.5f ? 1 : -1;
        Vector2 direccionVector = new Vector2(Mathf.Cos(angulo) * direccionX, Mathf.Sin(angulo) * direccionY);
        
        rb.linearVelocity = direccionVector * fuerzaInicial;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pala")) // Tag genérico para todas las palas
        {
            AudioManager.Instance.PlaySound(sfxPaddle, 0.2f);
            IncrementarVelocidad();
        }
        else
        {
            AudioManager.Instance.PlaySound(sfxWall, 0.2f);
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
