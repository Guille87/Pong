using System.Collections;
using UnityEngine;

public class PelotaController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxWall;
    [SerializeField] AudioClip sfxFail;

    Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] GameManager manager;
    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] float speedIncrement = 1.05f; // Incremento del 5% por rebote

    [SerializeField] float minY = -2.5f;
    [SerializeField] float maxY = 2.5f;

    const float MIN_ANG = 25.0f;
    const float MAX_ANG = 40.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        if (manager == null) {
            manager = Object.FindFirstObjectByType<GameManager>();
        }
        int direccionX = Random.Range(0, 2) == 0 ? -1 : 1;
        StartCoroutine(LanzarPelota(direccionX));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Pala1" || tag == "Pala2" || tag == "Pala3" || tag == "Pala4")
        {
            AudioManager.PlaySound(sfxPaddle, 0.2f);
            IncrementarVelocidad();
        }
        else if (tag == "LimiteSuperior" || tag == "LimiteInferior" || tag == "Circulos")
        {
            AudioManager.PlaySound(sfxWall, 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Porteria2")) {
            AudioManager.PlaySound(sfxFail, 0.2f);
            manager.addPointP1();

            if (manager.IsGameOver()) return;

            StartCoroutine(LanzarPelota(1));
        }
        else if(other.CompareTag("Porteria1")) {
            AudioManager.PlaySound(sfxFail, 0.2f);
            manager.addPointP2();
            
            if (manager.IsGameOver()) return;

            StartCoroutine(LanzarPelota(-1));
        }
    }

    IEnumerator LanzarPelota(int direccionX)
    {
        yield return new WaitForSeconds(delay);
        
        // Cálculo de la posición vertical del lanzamiento
        float posY = Random.Range(minY, maxY);
        transform.position = new Vector3(0, posY, 0);

        Vector2 impulso = ObtenerDireccionAleatoria(direccionX);

        // Resetear la velocidad lineal de la pelota
        rb.linearVelocity = Vector2.zero;

        // Aplicamos el impulso
        rb.AddForce(impulso * force, ForceMode2D.Impulse);
    }

    public void ResetPelota(int direccionX)
    {
        // Detén la velocidad actual
        rb.linearVelocity = Vector2.zero;

        // Reposiciona la pelota al centro
        transform.position = Vector3.zero;

        // Opcional: lanza la pelota automáticamente o espera a que el jugador la active
        StartCoroutine(LanzarPelota(direccionX));
    }

    private Vector2 ObtenerDireccionAleatoria(int direccionX)
    {
        // Cálculo del vector del lanzamiento
        float angulo = Random.Range(MIN_ANG, MAX_ANG) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angulo) * direccionX;
        int direccionY = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Mathf.Sin(angulo) * direccionY;
        return new Vector2(x, y);
    }

    private void IncrementarVelocidad()
    {
        // Incrementa la velocidad actual de la pelota
        Vector2 velocidadActual = rb.linearVelocity;
        rb.linearVelocity = velocidadActual * speedIncrement;
    }
}
